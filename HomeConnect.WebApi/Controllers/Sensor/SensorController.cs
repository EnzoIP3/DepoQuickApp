using BusinessLogic.Devices.Services;
using BusinessLogic.Notifications.Services;
using HomeConnect.WebApi.Controllers.Device;
using HomeConnect.WebApi.Controllers.Device.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Sensor;

[ApiController]
[Route("sensors")]
[AuthenticationFilter]
public class SensorController(INotificationService notificationService, IDeviceService deviceService) : BaseDeviceController(deviceService)
{
    [HttpPost("{hardwareId}/open")]
    public NotifyResponse NotifyOpen([FromRoute] string hardwareId)
    {
        NotificationArgs notificationArgs = CreateOpenNotificationArgs(hardwareId);
        notificationService.Notify(notificationArgs);
        return new NotifyResponse { HardwareId = hardwareId };
    }

    private static NotificationArgs CreateOpenNotificationArgs(string hardwareId)
    {
        var notificationArgs = new NotificationArgs
        {
            HardwareId = hardwareId,
            Date = DateTime.Now,
            Event = "open"
        };
        return notificationArgs;
    }

    [HttpPost("{hardwareId}/close")]
    public NotifyResponse NotifyClose([FromRoute] string hardwareId)
    {
        NotificationArgs notificationArgs = CreateCloseNotificationArgs(hardwareId);
        notificationService.Notify(notificationArgs);
        return new NotifyResponse { HardwareId = hardwareId };
    }

    private static NotificationArgs CreateCloseNotificationArgs(string hardwareId)
    {
        var notificationArgs = new NotificationArgs
        {
            HardwareId = hardwareId,
            Date = DateTime.Now,
            Event = "close"
        };
        return notificationArgs;
    }
}
