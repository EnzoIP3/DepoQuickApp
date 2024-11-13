using BusinessLogic;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;
using BusinessLogic.Devices.Services;
using HomeConnect.WebApi.Controllers.Devices.Models;
using HomeConnect.WebApi.Controllers.Homes.Models;
using HomeConnect.WebApi.Controllers.Rooms.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;
using GetDevicesResponse = HomeConnect.WebApi.Controllers.Devices.Models.GetDevicesResponse;

namespace HomeConnect.WebApi.Controllers.Devices;

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
        var args = new GetDevicesArgs
        {
            BusinessNameFilter = parameters.BusinessName,
            DeviceTypeFilter = parameters.Type,
            Page = parameters.Page,
            PageSize = parameters.PageSize,
            DeviceNameFilter = parameters.Name,
            ModelNumberFilter = parameters.Model
        };
        PagedData<Device> devices = _deviceService.GetDevices(args);
        GetDevicesResponse response = ResponseFromDevices(devices);
        return response;
    }

    private static GetDevicesResponse ResponseFromDevices(PagedData<Device> devices)
    {
        return new GetDevicesResponse
        {
            Devices = devices.Data.Select(d => new ListDeviceInfo
            {
                HardwareId = d.Id.ToString(),
                Name = d.Name,
                BusinessName = d.Business.Name,
                Type = d.Type.ToString(),
                ModelNumber = d.ModelNumber,
                Photo = d.MainPhoto
            }).ToList(),
            Pagination = new Pagination
            {
                Page = devices.Page,
                PageSize = devices.PageSize,
                TotalPages = devices.TotalPages
            }
        };
    }

    [HttpPost("{hardwareId}/turn_on")]
    public virtual ConnectionResponse TurnOn([FromRoute] string hardwareId)
    {
        var connectionState = _deviceService.TurnDevice(hardwareId, true);
        return new ConnectionResponse { Connected = connectionState, HardwareId = hardwareId };
    }

    [HttpPost("{hardwareId}/turn_off")]
    public virtual ConnectionResponse TurnOff([FromRoute] string hardwareId)
    {
        var connectionState = _deviceService.TurnDevice(hardwareId, false);
        return new ConnectionResponse { Connected = connectionState, HardwareId = hardwareId };
    }

    [HttpPatch("devices/{deviceId}/room")]
    public MoveDeviceResponse MoveDevice([FromRoute] string deviceId, [FromBody] MoveDeviceRequest request)
    {
        _deviceService.MoveDevice(request.SourceRoomId, request.TargetRoomId, deviceId);
        return new MoveDeviceResponse
        {
            SourceRoomId = request.SourceRoomId,
            TargetRoomId = request.TargetRoomId,
            DeviceId = deviceId
        };
    }
}
