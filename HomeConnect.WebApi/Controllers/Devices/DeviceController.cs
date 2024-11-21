using BusinessLogic;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Services;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using HomeConnect.WebApi.Controllers.Devices.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;
using GetDevicesResponse = HomeConnect.WebApi.Controllers.Devices.Models.GetDevicesResponse;

namespace HomeConnect.WebApi.Controllers.Devices;

[Route("devices")]
[ApiController]
public sealed class DeviceController : ControllerBase
{
    private readonly IDeviceService _deviceService;
    private readonly IHomeOwnerService _homeOwnerService;
    private readonly IImporterService _importerService;

    public DeviceController(IDeviceService deviceService,
        IImporterService importerService, IHomeOwnerService homeOwnerService)
    {
        _deviceService = deviceService;
        _importerService = importerService;
        _homeOwnerService = homeOwnerService;
    }

    [HttpGet]
    [AuthenticationFilter]
    public GetDevicesResponse GetDevices([FromQuery] GetDevicesRequest request)
    {
        PagedData<Device> devices = _deviceService.GetDevices(request.ToGetDevicesArgs());
        return GetDevicesResponse.FromDevices(devices);
    }

    [HttpPost]
    [AuthenticationFilter]
    [AuthorizationFilter(SystemPermission.ImportDevices)]
    public ImportDevicesResponse ImportDevices([FromBody] ImportDevicesRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        List<string> addedDevices = _importerService.ImportDevices(request.ToImportDevicesArgs(userLoggedIn));
        return new ImportDevicesResponse { ImportedDevices = addedDevices };
    }

    [HttpPost("{hardwareId}/turn_on")]
    public ConnectionResponse TurnOn([FromRoute] string hardwareId)
    {
        var connectionState = _deviceService.TurnDevice(hardwareId, true);
        return new ConnectionResponse { Connected = connectionState, HardwareId = hardwareId };
    }

    [HttpPost("{hardwareId}/turn_off")]
    public ConnectionResponse TurnOff([FromRoute] string hardwareId)
    {
        var connectionState = _deviceService.TurnDevice(hardwareId, false);
        return new ConnectionResponse { Connected = connectionState, HardwareId = hardwareId };
    }

    [HttpPatch("{hardwareId}/room")]
    [AuthenticationFilter]
    [AuthorizationFilter(SystemPermission.MoveDevice)]
    [HomeAuthorizationFilter(HomePermission.MoveDevice)]
    public MoveDeviceResponse MoveDevice([FromRoute] string hardwareId, [FromBody] MoveDeviceRequest request)
    {
        _deviceService.MoveDevice(request.TargetRoomId ?? string.Empty,
            hardwareId);
        return new MoveDeviceResponse { TargetRoomId = request.TargetRoomId!, DeviceId = hardwareId };
    }

    [HttpPatch("{hardwareId}/name")]
    [AuthenticationFilter]
    [AuthorizationFilter(SystemPermission.NameDevice)]
    [HomeAuthorizationFilter(HomePermission.NameDevice)]
    public NameDeviceResponse NameDevice([FromRoute] string hardwareId, [FromBody] NameDeviceRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        _homeOwnerService.NameDevice(request.ToNameDeviceArgs(userLoggedIn!, hardwareId));
        return new NameDeviceResponse { DeviceId = hardwareId };
    }
}
