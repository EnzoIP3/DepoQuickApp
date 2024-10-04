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
        PagedData<BusinessLogic.Devices.Entities.Device> devices = _deviceService.GetDevices(deviceName, device, model, business, page, pageSize);
        var response = ResponseFromDevices(devices);
        return Ok(response);
    }

    private static object ResponseFromDevices(PagedData<BusinessLogic.Devices.Entities.Device> devices)
    {
        var response = new
        {
            devices.Data,
            Pagination = new Pagination
            {
                Page = devices.Page,
                PageSize = devices.PageSize,
                TotalPages = devices.TotalPages
            }
        };
        return response;
    }
}
