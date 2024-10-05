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
    [HttpGet]
    public GetDeviceTypesResponse GetDeviceTypes()
    {
        var deviceTypes = deviceService.GetAllDeviceTypes();
        return new GetDeviceTypesResponse
        {
            DeviceTypes = deviceTypes.ToList()
        };
    }
}
