using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Services;
using BusinessLogic.Notifications.Models;
using BusinessLogic.Notifications.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using HomeConnect.WebApi.Controllers.Devices.Models;
using HomeConnect.WebApi.Controllers.Sensors.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Sensors;

[ApiController]
[Route("sensors")]
public sealed class SensorController
    : ControllerBase
{
    private readonly IBusinessOwnerService _businessOwnerService;
    private readonly IDeviceService _deviceService;
    private readonly INotificationService _notificationService;

    public SensorController(IBusinessOwnerService businessOwnerService, IDeviceService deviceService,
        INotificationService notificationService)
    {
        _businessOwnerService = businessOwnerService;
        _deviceService = deviceService;
        _notificationService = notificationService;
    }

    [HttpPost]
    [AuthenticationFilter]
    [AuthorizationFilter(SystemPermission.CreateSensor)]
    public CreateSensorResponse CreateSensor([FromBody] CreateSensorRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        Device createdSensor = _businessOwnerService.CreateDevice(request.ToArgs(userLoggedIn!));
        return new CreateSensorResponse { Id = createdSensor.Id };
    }

    [HttpPost("{hardwareId}/open")]
    public NotifyResponse Open([FromRoute] string hardwareId)
    {
        NotificationArgs notificationArgs = CreateOpenNotificationArgs(hardwareId);
        _notificationService.SendSensorNotification(notificationArgs, true);
        _deviceService.UpdateSensorState(hardwareId, true);
        return new NotifyResponse { HardwareId = hardwareId };
    }

    private static NotificationArgs CreateOpenNotificationArgs(string hardwareId)
    {
        var notificationArgs =
            new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "Sensor was opened" };
        return notificationArgs;
    }

    [HttpPost("{hardwareId}/close")]
    public NotifyResponse Close([FromRoute] string hardwareId)
    {
        NotificationArgs notificationArgs = CreateCloseNotificationArgs(hardwareId);
        _notificationService.SendSensorNotification(notificationArgs, false);
        _deviceService.UpdateSensorState(hardwareId, false);
        return new NotifyResponse { HardwareId = hardwareId };
    }

    private static NotificationArgs CreateCloseNotificationArgs(string hardwareId)
    {
        var notificationArgs =
            new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "Sensor was closed" };
        return notificationArgs;
    }
}
