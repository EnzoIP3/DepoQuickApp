using BusinessLogic.Auth.Models;
using BusinessLogic.Auth.Services;
using HomeConnect.WebApi.Controllers.Business.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Auth;

[ApiController]
[Route("auth")]
[AuthorizationFilter]
public class AuthController(IAuthService authService)
{
    private readonly IAuthService _authService = authService;

    [HttpPost("token")]
    public CreateTokenResponse CreateToken([FromBody] CreateTokenRequest request)
    {
        var args = new CreateTokenArgs() { Email = request.Email, Password = request.Password };
        var token = _authService.CreateToken(args);
        return new CreateTokenResponse() { Token = token };
    }
}
