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
public class CameraController(
    INotificationService notificationService,
    IDeviceService deviceService,
    IBusinessOwnerService businessOwnerService) : ControllerBase
{
    [HttpPost]
    [AuthenticationFilter]
    [AuthorizationFilter(SystemPermission.CreateCamera)]
    public CreateCameraResponse CreateCamera([FromBody] CreateCameraRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;

        var args = new CreateCameraArgs
        {
            Owner = userLoggedIn!,
            Name = request.Name ?? string.Empty,
            Description = request.Description ?? string.Empty,
            Exterior = request.Exterior,
            Interior = request.Interior,
            MainPhoto = request.MainPhoto ?? string.Empty,
            ModelNumber = request.ModelNumber,
            MotionDetection = request.MotionDetection,
            PersonDetection = request.PersonDetection,
            SecondaryPhotos = request.SecondaryPhotos,
        };

        Camera createdCamera = businessOwnerService.CreateCamera(args);

        return new CreateCameraResponse { Id = createdCamera.Id };
    }

    [HttpGet("{cameraId}")]
    [AuthenticationFilter]
    [AuthorizationFilter(SystemPermission.GetCamera)]
    public GetCameraResponse GetCamera([FromRoute] string cameraId)
    {
        Camera camera = deviceService.GetCameraById(cameraId);
        return new GetCameraResponse
        {
            Id = camera.Id.ToString(),
            Name = camera.Name,
            Description = camera.Description,
            Exterior = camera.IsExterior,
            Interior = camera.IsInterior,
            MainPhoto = camera.MainPhoto,
            ModelNumber = camera.ModelNumber,
            MotionDetection = camera.MotionDetection,
            PersonDetection = camera.PersonDetection,
            SecondaryPhotos = camera.SecondaryPhotos,
        };
    }

    [HttpPost("{hardwareId}/movement-detected")]
    public NotifyResponse MovementDetected([FromRoute] string hardwareId)
    {
        NotificationArgs args = CreateMovementDetectedNotificationArgs(hardwareId);
        notificationService.Notify(args, deviceService);
        return new NotifyResponse { HardwareId = hardwareId };
    }

    private static NotificationArgs CreateMovementDetectedNotificationArgs(string hardwareId)
    {
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "Movement detected" };
        return args;
    }

    [HttpPost("{hardwareId}/person-detected")]
    public NotifyResponse PersonDetected([FromRoute] string hardwareId, [FromBody] PersonDetectedRequest request)
    {
        NotificationArgs args = CreatePersonDetectedNotificationArgs(hardwareId, request.UserEmail ?? string.Empty);
        notificationService.Notify(args, deviceService);
        return new NotifyResponse { HardwareId = hardwareId };
    }

    private static NotificationArgs CreatePersonDetectedNotificationArgs(string hardwareId, string userEmail)
    {
        var args = new NotificationArgs
        {
            HardwareId = hardwareId, Date = DateTime.Now, Event = $"Person detected with email: {userEmail}"
        };
        return args;
    }
}
