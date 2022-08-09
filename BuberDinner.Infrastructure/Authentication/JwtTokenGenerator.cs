using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Services;
using Microsoft.IdentityModel.Tokens;

namespace BuberDinner.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public JwtTokenGenerator(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public string GenerateToken(Guid userId, string firstName, string lastName)
    {
        var signingCreds = new SigningCredentials(new SymmetricSecurityKey
                     (Encoding.UTF8.GetBytes("super-secret-key")), SecurityAlgorithms.HmacSha256);

        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, firstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, lastName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())  //Jti is JWT id
        };

        var securityToken = new JwtSecurityToken(
            issuer: "Buber-Diner",
            expires: _dateTimeProvider.UtcNow.AddMinutes(60),
            claims: claims,
            signingCredentials: signingCreds
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken); 

    }
}