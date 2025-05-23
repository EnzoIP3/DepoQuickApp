using BusinessLogic.Auth.Services;
using BusinessLogic.Users.Entities;
using HomeConnect.WebApi.Controllers.Auth.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Auth;

[ApiController]
[Route("auth")]
public sealed class AuthController : ControllerBase
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
        User user = _authService.GetUserFromToken(token);
        return CreateTokenResponse.FromUserAndToken(token, user);
    }
}
