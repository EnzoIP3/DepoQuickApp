using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Services;
using BusinessLogic.Notifications.Models;
using BusinessLogic.Notifications.Services;
using BusinessLogic.Roles.Entities;
using HomeConnect.WebApi.Controllers.Device;
using HomeConnect.WebApi.Controllers.Device.Models;
using HomeConnect.WebApi.Controllers.Sensor.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Sensor;

[ApiController]
[Route("sensors")]
[AuthenticationFilter]
public class SensorController(
    INotificationService notificationService,
    IDeviceService deviceService,
    IBusinessOwnerService businessOwnerService)
    : BaseDeviceController(deviceService)
{
    [HttpPost]
    [AuthorizationFilter(SystemPermission.CreateSensor)]
    public CreateSensorResponse CreateSensor([FromBody] CreateSensorRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as BusinessLogic.Users.Entities.User;
        var args = new CreateDeviceArgs()
        {
            Owner = userLoggedIn!,
            Description = request.Description,
            MainPhoto = request.MainPhoto,
            ModelNumber = request.ModelNumber,
            Name = request.Name,
            SecondaryPhotos = request.SecondaryPhotos,
            Type = "Sensor"
        };

        var createdSensor = businessOwnerService.CreateDevice(args);

        return new CreateSensorResponse { Id = createdSensor.Id };
    }

    [HttpPost("{hardwareId}/open")]
    public NotifyResponse NotifyOpen([FromRoute] string hardwareId)
    {
        NotificationArgs notificationArgs = CreateOpenNotificationArgs(hardwareId);
        notificationService.Notify(notificationArgs);
        return new NotifyResponse { HardwareId = hardwareId };
    }

    private static NotificationArgs CreateOpenNotificationArgs(string hardwareId)
    {
        var notificationArgs = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "open" };
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
        var notificationArgs = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "close" };
        return notificationArgs;
    }
}
