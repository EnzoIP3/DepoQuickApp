using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Services;
using BusinessLogic.Notifications.Models;
using BusinessLogic.Notifications.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Services;
using HomeConnect.WebApi.Controllers.Cameras.Models;
using HomeConnect.WebApi.Controllers.Devices;
using HomeConnect.WebApi.Controllers.Devices.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Cameras;

[ApiController]
[Route("cameras")]
public class CameraController(
    INotificationService notificationService,
    IDeviceService deviceService,
    IBusinessOwnerService businessOwnerService,
    IUserService userService)
    : BaseDeviceController(deviceService)
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
            Validator = request.Validator
        };

        Camera createdCamera = businessOwnerService.CreateCamera(args);

        return new CreateCameraResponse { Id = createdCamera.Id };
    }

    [HttpPost("{hardwareId}/movement-detected")]
    public NotifyResponse MovementDetected([FromRoute] string hardwareId)
    {
        EnsureDeviceIsConnected(hardwareId);
        NotificationArgs args = CreateMovementDetectedNotificationArgs(hardwareId);
        notificationService.Notify(args);
        return new NotifyResponse { HardwareId = hardwareId };
    }

    private void EnsureDeviceIsConnected(string hardwareId)
    {
        if (!deviceService.IsConnected(hardwareId))
        {
            throw new ArgumentException("Device is not connected");
        }
    }

    private static NotificationArgs CreateMovementDetectedNotificationArgs(string hardwareId)
    {
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "movement-detected" };
        return args;
    }

    [HttpPost("{hardwareId}/person-detected")]
    public NotifyResponse PersonDetected([FromRoute] string hardwareId, [FromBody] PersonDetectedRequest request)
    {
        EnsureDeviceIsConnected(hardwareId);
        NotificationArgs args = CreatePersonDetectedNotificationArgs(hardwareId, request.UserId ?? string.Empty);
        EnsureDetectedUserIsRegistered(request.UserId ?? string.Empty);
        notificationService.Notify(args);
        return new NotifyResponse { HardwareId = hardwareId };
    }

    private void EnsureDetectedUserIsRegistered(string requestUserId)
    {
        if (!userService.Exists(requestUserId))
        {
            throw new ArgumentException("User detected by camera is not found");
        }
    }

    private static NotificationArgs CreatePersonDetectedNotificationArgs(string hardwareId, string userId)
    {
        var args = new NotificationArgs
        {
            HardwareId = hardwareId,
            Date = DateTime.Now,
            Event = $"person detected with id: {userId}"
        };
        return args;
    }
}
