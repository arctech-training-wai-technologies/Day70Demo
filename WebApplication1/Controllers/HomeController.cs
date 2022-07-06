using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Services.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WeatherApiClientService _weatherApiClientService;

        public HomeController(ILogger<HomeController> logger, WeatherApiClientService weatherApiClientService)
        {
            _logger = logger;
            _weatherApiClientService = weatherApiClientService;
        }

        public async Task<IActionResult> Index()
        {
            // If token found in session and not yet expired then use it else
            //call getauthtoken api

            var token = await LoadAuthTokenAsync();

            ViewData["Token"] = token;

            //save this token in session

            var weatherInfos = await _weatherApiClientService.GetWeather(token);

            return View(weatherInfos);
        }

        private string? GetTokenFromSession()
        {
            var authInfo = HttpContext.Session.GetString("AuthTokenInfo");

            if (authInfo != null) // Found in session
            {
                var authResponse = JsonConvert.DeserializeObject<AuthResponse>(authInfo);

                if (authResponse.ExpiryDateTime > DateTime.Now)
                    return authResponse.Token;
            }

            return null;
        }

        private async Task<string> LoadAuthTokenAsync()
        {
            var token = GetTokenFromSession();

            if (token == null)
            {
                var authResponse = await _weatherApiClientService.GetAuthToken();

                var jsonAuthInfo = JsonConvert.SerializeObject(authResponse);
                HttpContext.Session.SetString("AuthTokenInfo", jsonAuthInfo);

                token = authResponse.Token;
            }

            return token;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}