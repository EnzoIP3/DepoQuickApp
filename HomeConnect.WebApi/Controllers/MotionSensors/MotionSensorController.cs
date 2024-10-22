using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Services;
using BusinessLogic.Notifications.Models;
using BusinessLogic.Notifications.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using HomeConnect.WebApi.Controllers.Devices.Models;
using HomeConnect.WebApi.Controllers.MotionSensors.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.MotionSensors;

public class MotionSensorController(
    INotificationService notificationService,
    IDeviceService deviceService,
    IBusinessOwnerService businessOwnerService)
    : ControllerBase
{
    [HttpPost]
    [AuthenticationFilter]
    [AuthorizationFilter(SystemPermission.CreateMotionSensor)]
    public CreateMotionSensorResponse CreateMotionSensor([FromBody] CreateMotionSensorRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        var args = new CreateDeviceArgs
        {
            Owner = userLoggedIn!,
            Description = request.Description ?? string.Empty,
            MainPhoto = request.MainPhoto ?? string.Empty,
            ModelNumber = request.ModelNumber,
            Name = request.Name ?? string.Empty,
            SecondaryPhotos = request.SecondaryPhotos,
            Type = "MotionSensor"
        };

        Device createdSensor = businessOwnerService.CreateDevice(args);

        return new CreateMotionSensorResponse { Id = createdSensor.Id };
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
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "movement-detected" };
        return args;
    }

    private void EnsureDeviceIsConnected(string hardwareId)
    {
        if (!deviceService.IsConnected(hardwareId))
        {
            throw new ArgumentException("Device is not connected");
        }
    }
}
