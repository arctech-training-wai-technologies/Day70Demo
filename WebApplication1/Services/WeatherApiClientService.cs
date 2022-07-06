namespace WebApplication1.Services;

public class WeatherApiClientService : IWeatherApiClientService
{
    private readonly HttpClient _httpClient;

    private const string AuthorizationKey = "Authorization";

    public WeatherApiClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://localhost:7075/");

        //var token = HttpContext.Session.GetString("Token");

        //_httpClient.DefaultRequestHeaders.Add(AuthorizationKey, $"Bearer {token}");
    }
}