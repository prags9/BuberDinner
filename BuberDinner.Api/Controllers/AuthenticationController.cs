using BuberDinner.Api.Controllers;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Authentication.Queries.Login;
using BuberDinner.Contracts.Authentication;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controller;

[Route("auth")]
public class AuthenticationController : ApiController
{
    private readonly ISender _mediator;

    public AuthenticationController(ISender _mediator)
    {
        this._mediator = _mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = new RegisterCommand(request.FirstName,request.LastName,request.Email,request.Password);
        ErrorOr<AuthenticationResult> authResult = await _mediator.Send(command);//_authenticationService.Register(request.FirstName, request.LastName, request.Email, request.Password);

        return authResult.Match(
            authResult => Ok(MapAuthResult(authResult)),
              errors => problem(errors)

        );
    }

    private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(
            authResult.User.Id,
            authResult.User.FirstName,
            authResult.User.Lastname,
            authResult.User.Email,
            authResult.Token);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = new LoginQuery(request.Email, request.Password);
        var authResult = await _mediator.Send(query);

    if(authResult.IsError && authResult.FirstError == Domain.Common.Errors.Errors.Authentication.InvalidCredentials){
        return Problem(statusCode: StatusCodes.Status401Unauthorized,
        title: authResult.FirstError.Description);
    }

       return authResult.Match(
            authResult => Ok(MapAuthResult(authResult)),
              errors => problem(errors)

        );
    }
}