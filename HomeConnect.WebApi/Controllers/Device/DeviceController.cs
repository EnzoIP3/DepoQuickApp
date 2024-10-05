using BusinessLogic;
using BusinessLogic.Devices.Models;
using BusinessLogic.Devices.Services;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Device;

[Route("devices")]
[ApiController]
[AuthenticationFilter]
public class DeviceController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DeviceController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpGet]
    public IActionResult GetDevices([FromQuery] GetDeviceArgs parameters)
    {
        PagedData<BusinessLogic.Devices.Entities.Device> devices = _deviceService.GetDevices(parameters);
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
                Page = devices.Page, PageSize = devices.PageSize, TotalPages = devices.TotalPages
            }
        };
        return response;
    }
}
