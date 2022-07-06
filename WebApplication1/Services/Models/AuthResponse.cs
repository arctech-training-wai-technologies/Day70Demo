namespace WebApplication1.Services.Models;

public class AuthResponse
{
    public string Token { get; set; } = null!;
    public DateTime ExpiryDateTime { get; set; }
}