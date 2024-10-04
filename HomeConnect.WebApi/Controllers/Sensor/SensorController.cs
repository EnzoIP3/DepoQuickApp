using BusinessLogic.Devices.Services;
using BusinessLogic.Notifications.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Sensor;

[ApiController]
[Route("sensors")]
public class SensorController(INotificationService notificationService, IDeviceService deviceService) : ControllerBase
{
    [HttpPost("{hardwareId}/open")]
    public NotifyResponse NotifyOpen([FromRoute] string hardwareId)
    {
        var notificationArgs = new NotificationArgs
        {
            HardwareId = hardwareId,
            Date = DateTime.Now,
            Event = "open"
        };
        notificationService.Notify(notificationArgs);
        return new NotifyResponse { HardwareId = hardwareId };
    }

    [HttpPost("{hardwareId}/close")]
    public NotifyResponse NotifyClose([FromRoute] string hardwareId)
    {
        var notificationArgs = new NotificationArgs
        {
            HardwareId = hardwareId,
            Date = DateTime.Now,
            Event = "close"
        };
        notificationService.Notify(notificationArgs);
        return new NotifyResponse { HardwareId = hardwareId };
    }

    [HttpPost("{hardwareId}/toggle")]
    public ConnectionResponse Toggle([FromRoute] string hardwareId)
    {
        var connectionState = deviceService.Toogle(hardwareId);
        return new ConnectionResponse { ConnectionState = connectionState, HardwareId = hardwareId };
    }
}
