using BusinessLogic;
using BusinessLogic.Devices.Models;
using BusinessLogic.Devices.Services;
using HomeConnect.WebApi.Controllers.Device.Models;
using HomeConnect.WebApi.Controllers.Home.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;
using GetDevicesResponse = HomeConnect.WebApi.Controllers.Device.Models.GetDevicesResponse;

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
    public GetDevicesResponse GetDevices([FromQuery] GetDevicesRequest parameters)
    {
        var args = new GetDevicesArgs()
        {
            BusinessNameFilter = parameters.BusinessNameFilter,
            DeviceTypeFilter = parameters.DeviceTypeFilter,
            Page = parameters.Page,
            PageSize = parameters.PageSize,
            DeviceNameFilter = parameters.DeviceNameFilter,
            ModelNumberFilter = parameters.ModelNumberFilter
        };
        var devices = _deviceService.GetDevices(args);
        var response = ResponseFromDevices(devices);
        return response;
    }

    private static GetDevicesResponse ResponseFromDevices(PagedData<BusinessLogic.Devices.Entities.Device> devices)
    {
        return new GetDevicesResponse()
        {
            Devices = devices.Data.Select(d => new ListDeviceInfo()
            {
                Name = d.Name,
                BusinessName = d.Business.Name,
                Type = d.Type.ToString(),
                ModelNumber = d.ModelNumber,
                Photo = d.MainPhoto,
                IsConnected = d.ConnectionState
            }).ToList(),
            Pagination = new Pagination()
            {
                Page = devices.Page, PageSize = devices.PageSize, TotalPages = devices.TotalPages
            }
        };
    }
}
