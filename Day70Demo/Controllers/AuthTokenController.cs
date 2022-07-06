using Day70Demo.Services;
using Day70Demo.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Day70Demo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthTokenController : ControllerBase
{
    private readonly IAuthTokenService _authTokenService;
    private readonly ILogger<AuthTokenController> _logger;

    public AuthTokenController(IAuthTokenService authTokenService, ILogger<AuthTokenController> logger)
    {
        _authTokenService = authTokenService;
        _logger = logger;
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult PostAsync(TokenCredential tokenCredential)
    {
        try
        {
            var authResponse = _authTokenService.GetAuthToken(tokenCredential.Username, tokenCredential.Password);

            return Ok(authResponse);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting Auth token");
            return BadRequest(e.Message);
        }
    }
}