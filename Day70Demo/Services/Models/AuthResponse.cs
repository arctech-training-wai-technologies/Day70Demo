namespace Day70Demo.Services.Models;

public class AuthResponse
{
    public string Token { get; set; }
    public DateTime ExpiryDateTime { get; set; }
}