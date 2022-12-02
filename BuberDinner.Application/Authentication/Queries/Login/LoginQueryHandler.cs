using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Entities;
using ErrorOr;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Application.Authentication.Common;
using MediatR;

namespace BuberDinner.Application.Authentication.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery,ErrorOr<AuthenticationResult>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public LoginQueryHandler(IJwtTokenGenerator _jwtTokenGenerator,IUserRepository userRepository)
        {
            this._jwtTokenGenerator = _jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        //1. Validate user exists.
        if(_userRepository.GetUserByEmail(query.Email) is not User user){
            return Errors.Authentication.InvalidCredentials;
        }
        //2. Validate the password is correct
        if(user.Password != query.Password){
            return Errors.Authentication.InvalidCredentials;
        }
        //3. Crate JWT Toekn and return it.
        var token = _jwtTokenGenerator.GenerateToken(user);

       return new AuthenticationResult(user,
                                       token);
        }
    }
}