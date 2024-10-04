using BusinessLogic.Auth.Models;
using BusinessLogic.Auth.Services;
using HomeConnect.WebApi.Controllers.Auth.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Auth;

[ApiController]
[Route("auth")]
[AuthorizationFilter]
public class AuthController(IAuthService authService)
{
    [HttpPost("token")]
    public CreateTokenResponse CreateToken([FromBody] CreateTokenRequest request)
    {
        var args = new CreateTokenArgs() { Email = request.Email, Password = request.Password };
        var token = authService.CreateToken(args);
        return new CreateTokenResponse() { Token = token };
    }
}
