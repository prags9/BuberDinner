using BuberDinner.Api.Controllers;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Authentication.Queries.Login;
using BuberDinner.Contracts.Authentication;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controller;

[Route("auth")]
public class AuthenticationController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public AuthenticationController(ISender _mediator,IMapper _mapper)
    {
        this._mediator = _mediator;
        this._mapper = _mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = _mapper.Map<RegisterCommand>(request);
        ErrorOr<AuthenticationResult> authResult = await _mediator.Send(command);//_authenticationService.Register(request.FirstName, request.LastName, request.Email, request.Password);

        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
              errors => problem(errors)

        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = _mapper.Map<LoginQuery>(request);
        var authResult = await _mediator.Send(query);

    if(authResult.IsError && authResult.FirstError == Domain.Common.Errors.Errors.Authentication.InvalidCredentials){
        return Problem(statusCode: StatusCodes.Status401Unauthorized,
        title: authResult.FirstError.Description);
    }

       return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
              errors => problem(errors)

        );
    }
}