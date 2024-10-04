using BusinessLogic;
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
    public IActionResult GetDevices([FromQuery] string? deviceName = null, [FromQuery] string? device = null, [FromQuery] string? model = null, [FromQuery] string? business = null, [FromQuery] int? page = 1, [FromQuery] int? pageSize = 20, [FromQuery]  string? deviceNameFilter = null)
    {
        PagedData<BusinessLogic.Devices.Entities.Device> devices = _deviceService.GetDevices(page, pageSize, deviceNameFilter);
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
