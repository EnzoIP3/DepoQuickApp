using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Services;
using BusinessLogic.Notifications.Models;
using BusinessLogic.Notifications.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Services;
using HomeConnect.WebApi.Controllers.Camera.Models;
using HomeConnect.WebApi.Controllers.Device;
using HomeConnect.WebApi.Controllers.Device.Models;
using HomeConnect.WebApi.Controllers.Sensor;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Camera;

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
    [AuthorizationFilter(SystemPermission.CreateCamera)]
    public CreateCameraResponse CreateCamera([FromBody] CreateCameraRequest request)
    {
        var args = new CreateCameraArgs()
        {
            Name = request.Name,
            BusinessRut = request.BusinessRut,
            Description = request.Description,
            IsExterior = request.IsExterior,
            IsInterior = request.IsInterior,
            MainPhoto = request.MainPhoto,
            ModelNumber = request.ModelNumber,
            MotionDetection = request.MotionDetection,
            PersonDetection = request.PersonDetection,
            SecondaryPhotos = request.SecondaryPhotos
        };

        var createdCamera = businessOwnerService.CreateCamera(args);

        return new CreateCameraResponse { Id = createdCamera };
    }

    [HttpPost("{hardwareId}/movement-detected")]
    public NotifyResponse MovementDetected([FromRoute] string hardwareId)
    {
        NotificationArgs args = CreateMovementDetectedNotificationArgs(hardwareId);
        notificationService.Notify(args);
        return new NotifyResponse { HardwareId = hardwareId };
    }

    private static NotificationArgs CreateMovementDetectedNotificationArgs(string hardwareId)
    {
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "movement-detected" };
        return args;
    }

    [HttpPost("{hardwareId}/person-detected")]
    public NotifyResponse PersonDetected([FromRoute] string hardwareId, PersonDetectedRequest request)
    {
        NotificationArgs args = CreatePersonDetectedNotificationArgs(hardwareId, request.UserId);
        EnsureDetectedUserIsRegistered(request.UserId);
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
            Event = $"person detected with id: {userId}",
        };
        return args;
    }
}
