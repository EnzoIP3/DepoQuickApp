using BusinessLogic.Notifications.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Sensor;

[ApiController]
[Route("sensors")]
public class SensorController(INotificationService notificationService) : ControllerBase
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
}
