using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Services;
using BusinessLogic.Notifications.Models;
using BusinessLogic.Notifications.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using HomeConnect.WebApi.Controllers.Devices;
using HomeConnect.WebApi.Controllers.Devices.Models;
using HomeConnect.WebApi.Controllers.Sensors.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Sensors;

[ApiController]
[Route("sensors")]
public class SensorController(
    INotificationService notificationService,
    IDeviceService deviceService,
    IBusinessOwnerService businessOwnerService)
    : BaseDeviceController(deviceService)
{
    [HttpPost]
    [AuthenticationFilter]
    [AuthorizationFilter(SystemPermission.CreateSensor)]
    public CreateSensorResponse CreateSensor([FromBody] CreateSensorRequest request)
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
            Type = "Sensor"
        };

        Device createdSensor = businessOwnerService.CreateDevice(args);

        return new CreateSensorResponse { Id = createdSensor.Id };
    }

    [HttpPost("{hardwareId}/open")]
    public NotifyResponse NotifyOpen([FromRoute] string hardwareId)
    {
        EnsureDeviceIsConnected(hardwareId);
        NotificationArgs notificationArgs = CreateOpenNotificationArgs(hardwareId);
        notificationService.Notify(notificationArgs);
        return new NotifyResponse { HardwareId = hardwareId };
    }

    private void EnsureDeviceIsConnected(string hardwareId)
    {
        if (!deviceService.IsConnected(hardwareId))
        {
            throw new ArgumentException("Device is not connected");
        }
    }

    private static NotificationArgs CreateOpenNotificationArgs(string hardwareId)
    {
        var notificationArgs = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "open" };
        return notificationArgs;
    }

    [HttpPost("{hardwareId}/close")]
    public NotifyResponse NotifyClose([FromRoute] string hardwareId)
    {
        EnsureDeviceIsConnected(hardwareId);
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
