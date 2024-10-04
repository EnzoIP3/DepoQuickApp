using BusinessLogic.Devices.Services;
using BusinessLogic.Notifications.Services;
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
        var args = new NotificationArgs
        {
            HardwareId = hardwareId,
            Date = DateTime.Now,
            Event = "movement-detected"
        };
        notificationService.Notify(args);
        return new NotifyResponse { HardwareId = hardwareId };
    }
}
