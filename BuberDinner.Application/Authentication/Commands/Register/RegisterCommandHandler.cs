using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Entities;
using ErrorOr;
using MediatR;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Application.Authentication.Common;

namespace BuberDinner.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public RegisterCommandHandler(IJwtTokenGenerator _jwtTokenGenerator,IUserRepository userRepository)
        {
            this._jwtTokenGenerator = _jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            //1. Validate the user doesn't exist.
        if(_userRepository.GetUserByEmail(command.Email) is not null){
            return Errors.User.DuplicateEmail;
        }
        //2. create user (and generate a unique id) & persist to DB.
        var user = new User{
            FirstName = command.FirstName,
            Lastname = command.LastName,
            Email = command.Email,
            Password = command.Password
        };

        _userRepository.Add(user);

        //3. create jwt token & return it.
        
        var token = _jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(
        user,
        token
       );
        }
    }
}