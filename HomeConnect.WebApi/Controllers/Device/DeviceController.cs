using BusinessLogic;
using BusinessLogic.Devices.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Device;

[Route("v1/[controller]")]
[ApiController]
public class DeviceController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DeviceController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpGet]
    public IActionResult GetDevices([FromQuery] string? deviceName = null, [FromQuery] string? device = null, [FromQuery] string? model = null, [FromQuery] string? business = null, [FromQuery] int? page = 1, [FromQuery] int? pageSize = 20)
    {
        throw new NotImplementedException();
    }
}
