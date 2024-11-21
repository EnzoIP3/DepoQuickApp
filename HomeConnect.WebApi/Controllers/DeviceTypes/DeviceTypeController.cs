using BusinessLogic.Devices.Services;
using HomeConnect.WebApi.Controllers.DeviceTypes.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.DeviceTypes;

[ApiController]
[Route("device_types")]
[AuthenticationFilter]
public class DeviceTypeController : ControllerBase
{
    private const string DefaultCacheTime = "2592000";
    private readonly IDeviceService _deviceService;

    public DeviceTypeController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpGet]
    public GetDeviceTypesResponse GetDeviceTypes()
    {
        IEnumerable<string> deviceTypes = _deviceService.GetAllDeviceTypes();
        Response.Headers.CacheControl = $"public,max-age={DefaultCacheTime}";
        return new GetDeviceTypesResponse { DeviceTypes = deviceTypes.ToList() };
    }
}
