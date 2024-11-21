using BusinessLogic;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;
using BusinessLogic.Devices.Services;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Models;
using BusinessLogic.HomeOwners.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using HomeConnect.WebApi.Controllers.Devices.Models;
using HomeConnect.WebApi.Controllers.HomeOwners.Models;
using HomeConnect.WebApi.Controllers.Homes.Models;
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
    private readonly IImporterService _importerService;
    private readonly IHomeOwnerService _homeOwnerService;

    public DeviceController(IDeviceService deviceService,
        IImporterService importerService, IHomeOwnerService homeOwnerService)
    {
        _deviceService = deviceService;
        _importerService = importerService;
        _homeOwnerService = homeOwnerService;
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
            ModelNumberFilter = parameters.ModelNumber
        };
        var devices = _deviceService.GetDevices(args);
        GetDevicesResponse response = ResponseFromDevices(devices);
        return response;
    }

    private static GetDevicesResponse ResponseFromDevices(PagedData<Device> devices)
    {
        return new GetDevicesResponse
        {
            Devices = devices.Data.Select(d => new ListDeviceInfo
            {
                Id = d.Id.ToString(),
                Name = d.Name,
                BusinessName = d.Business.Name,
                Type = d.Type.ToString(),
                ModelNumber = d.ModelNumber,
                MainPhoto = d.MainPhoto,
                SecondaryPhotos = d.SecondaryPhotos,
                Description = d.Description
            }).ToList(),
            Pagination = new Pagination
            {
                Page = devices.Page,
                PageSize = devices.PageSize,
                TotalPages = devices.TotalPages
            }
        };
    }

    [HttpPost]
    [AuthorizationFilter(SystemPermission.ImportDevices)]
    public ImportDevicesResponse ImportDevices([FromBody] ImportDevicesRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        var args = new ImportDevicesArgs
        {
            ImporterName = request.ImporterName,
            User = userLoggedIn!,
            Parameters = request.Parameters
        };
        var addedDevices = _importerService.ImportDevices(args);
        return new ImportDevicesResponse { ImportedDevices = addedDevices };
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

    [HttpPatch("{hardwareId}/room")]
    [AuthorizationFilter(SystemPermission.MoveDevice)]
    [HomeAuthorizationFilter(HomePermission.MoveDevice)]
    public MoveDeviceResponse MoveDevice([FromRoute] string hardwareId, [FromBody] MoveDeviceRequest request)
    {
        _deviceService.MoveDevice(request.TargetRoomId ?? string.Empty,
            hardwareId);
        return new MoveDeviceResponse { TargetRoomId = request.TargetRoomId!, DeviceId = hardwareId };
    }

    [HttpPatch("{hardwareId}/name")]
    [AuthorizationFilter(SystemPermission.NameDevice)]
    [HomeAuthorizationFilter(HomePermission.NameDevice)]
    public NameDeviceResponse NameDevice([FromRoute] string hardwareId, [FromBody] NameDeviceRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        var nameDeviceArgs = NameDeviceArgsFromRequest(request, userLoggedIn.Id, hardwareId);
        _homeOwnerService.NameDevice(nameDeviceArgs);
        return new NameDeviceResponse { DeviceId = hardwareId };
    }

    private static NameDeviceArgs NameDeviceArgsFromRequest(NameDeviceRequest request, Guid userId, string hardwareId)
    {
        if (string.IsNullOrEmpty(hardwareId))
        {
            throw new ArgumentException("DeviceId cannot be null or empty");
        }

        if (string.IsNullOrEmpty(request.NewName))
        {
            throw new ArgumentException("NewName cannot be null or empty");
        }

        return new NameDeviceArgs()
        {
            HardwareId = Guid.Parse(hardwareId),
            NewName = request.NewName,
            OwnerId = userId
        };
    }
}
