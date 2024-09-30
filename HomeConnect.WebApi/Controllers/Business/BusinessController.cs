using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers;

[ApiController]
[Route("businesses")]
[AuthorizationFilter]
public class BusinessController() : ControllerBase
{
    public IActionResult GetBusinesses([FromQuery] int? currentPage = null, [FromQuery] int? pageSize = null,
        [FromQuery] string? nameFilter = null, [FromQuery] string? ownerFilter = null)
    {
        throw new NotImplementedException();
    }
}
