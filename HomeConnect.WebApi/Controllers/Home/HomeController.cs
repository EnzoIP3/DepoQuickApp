using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Home;

[ApiController]
[Route("homes")]
[AuthorizationFilter]
public class HomeController() : ControllerBase
{
    [HttpPost]
    public CreateHomeResponse CreateHome([FromBody] CreateHomeRequest request)
    {
        throw new NotImplementedException();
    }
}
