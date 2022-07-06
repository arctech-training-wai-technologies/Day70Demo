using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Day70Demo.Services.Exceptions;
using Day70Demo.Services.Models;
using Day70Demo.Services.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Day70Demo.Services;

public class AuthTokenService : IAuthTokenService
{
    private readonly JwtSecuritySettings _jwtSecuritySettings;

    public AuthTokenService(IOptions<JwtSecuritySettings> jwtSecuritySettingOptions)
    {
        _jwtSecuritySettings = jwtSecuritySettingOptions.Value;
    }

    public AuthResponse GetAuthToken(string username, string password)
    {
        if (username != "abcdef" || password != "mno")
            throw new AuthFailedException("Invalid UserName and/or Password");

        // Get token and expiryDatetime
        var expiryDateTime = DateTime.Now.AddSeconds(_jwtSecuritySettings.TokenValiditySeconds);
        var tokenKey = Encoding.ASCII.GetBytes(_jwtSecuritySettings.SecurityKey);

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