namespace Day70Demo.Services.Settings;

public class JwtSecuritySettings
{
    public string SecurityKey { get; set; } = null!;
    public int TokenValiditySeconds { get; set; }
}