using BusinessLogic.Roles.Entities;
using HomeConnect.WebApi.Controllers.Lamps.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Lamps;

public class LampController : ControllerBase
{
    [HttpPost]
    [AuthenticationFilter]
    [AuthorizationFilter(SystemPermission.CreateLamp)]
    public CreateLampResponse CreateLamp([FromBody] CreateLampRequest request)
    {
        throw new NotImplementedException();
    }
}
