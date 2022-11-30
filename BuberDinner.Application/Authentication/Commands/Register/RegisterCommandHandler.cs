using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Services.Authentication;
using ErrorOr;
using MediatR;

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

        public Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            //1. Validate the user doesn't exist.
        if(_userRepository.GetUserByEmail(email) is not null){
            return Errors.User.DuplicateEmail;
        }
        //2. create user (and generate a unique id) & persist to DB.
        var user = new User{
            FirstName = firstName,
            Lastname = lastName,
            Email = email,
            Password = password
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