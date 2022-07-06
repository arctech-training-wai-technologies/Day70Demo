using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Day70Demo.Services.Exceptions;
using Day70Demo.Services.Models;
using Microsoft.IdentityModel.Tokens;

namespace Day70Demo.Services;

public class AuthTokenService : IAuthTokenService
{
    public AuthResponse GetAuthToken(string username, string password)
    {
        if (username != "abcdef" || password != "mno")
            throw new AuthFailedException("Invalid UserName and/or Password");

        // Get token and expiryDatetime
        var expiryDateTime = DateTime.Now.AddMinutes(5);
        var tokenKey = Encoding.ASCII.GetBytes(@"54d6504255f2effe17f74a8b8170e7a8ece0fc79");

        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new List<Claim>
            {
                new("userName", username),
                new("expiresIn", expiryDateTime.ToString()),
            }),
            Expires = expiryDateTime,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
        var token = jwtSecurityTokenHandler.WriteToken(securityToken);

        return new AuthResponse {Token = token, ExpiryDateTime = expiryDateTime};
        //return new AuthResponse {Token = "asdghfsgsdlhsgshgs", ExpiryDateTime = DateTime.Now.AddMinutes(5)};
    }
}