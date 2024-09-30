using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.User;

[ApiController]
[Route("admins")]
[AuthorizationFilter]
public class UserController() : ControllerBase
{
    [HttpGet]
    public IActionResult GetUsers()
    {
        throw new NotImplementedException();
    }
}
