using Day70Demo.Services.Models;

namespace Day70Demo.Services;

public interface IAuthTokenService
{
    public AuthResponse GetAuthToken(string username, string password);
}