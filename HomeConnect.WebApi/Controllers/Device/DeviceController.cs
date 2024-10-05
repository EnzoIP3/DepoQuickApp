using BusinessLogic;
using BusinessLogic.Devices.Models;
using HomeConnect.WebApi.Controllers.Device.Models;
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
    public IActionResult GetDevices([FromQuery] DeviceQueryParameters parameters)
    {
        PagedData<BusinessLogic.Devices.Entities.Device> devices = _deviceService.GetDevices(parameters.Page, parameters.PageSize, parameters.DeviceNameFilter, parameters.ModelNameFilter, parameters.BusinessNameFilter, parameters.DeviceTypeFilter);
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
