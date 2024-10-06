using BusinessLogic.Devices.Services;
using HomeConnect.WebApi.Controllers.DeviceTypes.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.DeviceTypes;

[ApiController]
[Route("device_types")]
[AuthenticationFilter]
public class DeviceTypesController(IDeviceService deviceService) : ControllerBase
{
    private readonly string _defaultCacheTime = "2592000";
    [HttpGet]
    public GetDeviceTypesResponse GetDeviceTypes()
    {
        var deviceTypes = deviceService.GetAllDeviceTypes();
        Response.Headers["Cache-Control"] = $"public,max-age={_defaultCacheTime}";
        return new GetDeviceTypesResponse { DeviceTypes = deviceTypes.ToList() };
    }
}
