using BusinessLogic.Auth.Models;
using BusinessLogic.Auth.Services;
using HomeConnect.WebApi.Controllers.Auth.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Auth;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public CreateTokenResponse CreateToken([FromBody] CreateTokenRequest request)
    {
        var token = _authService.CreateToken(request.ToCreateTokenArgs());
        var user = _authService.GetUserFromToken(token);
        return CreateTokenResponse.FromUserAndToken(token, user);
    }
}
