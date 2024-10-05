using BusinessLogic.Devices.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.DeviceTypes;

[ApiController]
[Route("device_types")]
public class DeviceTypesController(IDeviceService deviceService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetDeviceTypes()
    {
        var deviceTypes = deviceService.GetAllDeviceTypes();
        return Ok(deviceTypes);
    }
}
