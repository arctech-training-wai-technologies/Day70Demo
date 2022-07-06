using System.Drawing;
using System.Text;
using Newtonsoft.Json;
using WebApplication1.Services.Exceptions;
using WebApplication1.Services.Models;

namespace WebApplication1.Services;

public class WeatherApiClientService
{
    private readonly HttpClient _httpClient;

    private const string AuthorizationKey = "Authorization";

    public WeatherApiClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://localhost:7075/");


        //_httpClient.DefaultRequestHeaders.Add(AuthorizationKey, $"Bearer {token}");
    }

    public async Task<AuthResponse> GetAuthToken()
    {
        const string requestUrl = "api/AuthToken";

        var authDetails = new AuthDetails {Username = "abcdef", Password = "mno"};
        var jsonAuthDetails = JsonConvert.SerializeObject(authDetails,
            new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});

        var content = new StringContent(jsonAuthDetails, Encoding.UTF8, "application/json");

        using var response = await _httpClient.PostAsync(requestUrl, content);

        if (!response.IsSuccessStatusCode)
            throw new ApiResponseException("Api failed");

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

        if (authResponse == null)
            throw new ApiResponseException("Api returned invalid value");


        return authResponse;
    }

    public async Task<IEnumerable<WeatherInfo>> GetWeather(string token)
    {
        _httpClient.DefaultRequestHeaders.Add(AuthorizationKey, $"Bearer {token}");

        const string requestUrl = "WeatherForecast/GetWeatherForecast";

        using var response = await _httpClient.GetAsync(requestUrl);

        if (!response.IsSuccessStatusCode)
            throw new ApiResponseException($"Api failed, {response.ReasonPhrase}");

        var weatherInfos = await response.Content.ReadFromJsonAsync<IEnumerable<WeatherInfo>>();

        if (weatherInfos == null)
            throw new ApiResponseException("Api returned invalid value");

        return weatherInfos;
    }
}

public class WeatherInfo
{
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF { get; set; }
    public string Summary { get; set; } = null!;
}