using BusinessLogic.Devices.Services;
using BusinessLogic.Notifications.Services;
using HomeConnect.WebApi.Controllers.Camera.Models;
using HomeConnect.WebApi.Controllers.Device;
using HomeConnect.WebApi.Controllers.Device.Models;
using HomeConnect.WebApi.Controllers.Sensor;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Camera;

[ApiController]
[Route("cameras")]
public class CameraController(INotificationService notificationService, IDeviceService deviceService) : BaseDeviceController(deviceService)
{
    [HttpPost("{hardwareId}/movement-detected")]
    public NotifyResponse MovementDetected([FromRoute] string hardwareId)
    {
        NotificationArgs args = CreateMovementDetectedNotificationArgs(hardwareId);
        notificationService.Notify(args);
        return new NotifyResponse { HardwareId = hardwareId };
    }

    private static NotificationArgs CreateMovementDetectedNotificationArgs(string hardwareId)
    {
        var args = new NotificationArgs
        {
            HardwareId = hardwareId,
            Date = DateTime.Now,
            Event = "movement-detected"
        };
        return args;
    }

    [HttpPost("{hardwareId}/person-detected")]
    public NotifyResponse PersonDetected([FromRoute] string hardwareId, PersonDetectedRequest request)
    {
        NotificationArgs args = CreatePersonDetectedNotificationArgs(hardwareId, request.UserId);
        notificationService.Notify(args);
        return new NotifyResponse { HardwareId = hardwareId };
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
