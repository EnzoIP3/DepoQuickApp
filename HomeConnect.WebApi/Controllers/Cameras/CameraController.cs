using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Services;
using BusinessLogic.Notifications.Models;
using BusinessLogic.Notifications.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using HomeConnect.WebApi.Controllers.Cameras.Models;
using HomeConnect.WebApi.Controllers.Devices.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Cameras;

[ApiController]
[Route("cameras")]
public class CameraController : ControllerBase
{
    private readonly IDeviceService _deviceService;
    private readonly INotificationService _notificationService;
    private readonly IBusinessOwnerService _businessOwnerService;

    public CameraController(INotificationService notificationService, IDeviceService deviceService,
        IBusinessOwnerService businessOwnerService)
    {
        _notificationService = notificationService;
        _deviceService = deviceService;
        _businessOwnerService = businessOwnerService;
    }

    [HttpPost]
    [AuthenticationFilter]
    [AuthorizationFilter(SystemPermission.CreateCamera)]
    public CreateCameraResponse CreateCamera([FromBody] CreateCameraRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        Camera createdCamera = _businessOwnerService.CreateCamera(request.ToCreateCameraArgs(userLoggedIn));
        return new CreateCameraResponse { Id = createdCamera.Id };
    }

    [HttpGet("{cameraId}")]
    [AuthenticationFilter]
    [AuthorizationFilter(SystemPermission.GetCamera)]
    public GetCameraResponse GetCamera([FromRoute] string cameraId)
    {
        Camera camera = _deviceService.GetCameraById(cameraId);
        return GetCameraResponse.FromCamera(camera);
    }

    [HttpPost("{hardwareId}/movement-detected")]
    public NotifyResponse MovementDetected([FromRoute] string hardwareId)
    {
        var args = NotificationArgs.CreateMovementDetectedNotificationArgs(hardwareId);
        _notificationService.Notify(args);
        return new NotifyResponse { HardwareId = hardwareId };
    }

    [HttpPost("{hardwareId}/person-detected")]
    public NotifyResponse PersonDetected([FromRoute] string hardwareId, [FromBody] PersonDetectedRequest request)
    {
        var args = NotificationArgs.CreatePersonDetectedNotificationArgs(hardwareId, request.UserEmail ?? string.Empty);
        _notificationService.Notify(args);
        return new NotifyResponse { HardwareId = hardwareId };
    }
}
