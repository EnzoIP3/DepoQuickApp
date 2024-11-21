using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Notifications.Models;
using BusinessLogic.Notifications.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using HomeConnect.WebApi.Controllers.Devices.Models;
using HomeConnect.WebApi.Controllers.MotionSensors.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.MotionSensors;

[ApiController]
[Route("motion_sensors")]
public class MotionSensorController
    : ControllerBase
{
    private readonly IBusinessOwnerService _businessOwnerService;
    private readonly INotificationService _notificationService;

    public MotionSensorController(IBusinessOwnerService businessOwnerService, INotificationService notificationService)
    {
        _businessOwnerService = businessOwnerService;
        _notificationService = notificationService;
    }

    [HttpPost]
    [AuthenticationFilter]
    [AuthorizationFilter(SystemPermission.CreateMotionSensor)]
    public CreateMotionSensorResponse CreateMotionSensor([FromBody] CreateMotionSensorRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        var createdSensor = _businessOwnerService.CreateDevice(request.ToArgs(userLoggedIn!));
        return new CreateMotionSensorResponse { Id = createdSensor.Id };
    }

    [HttpPost("{hardwareId}/movement-detected")]
    public NotifyResponse MovementDetected([FromRoute] string hardwareId)
    {
        NotificationArgs args = CreateMovementDetectedNotificationArgs(hardwareId);
        _notificationService.Notify(args);
        return new NotifyResponse { HardwareId = hardwareId };
    }

    private static NotificationArgs CreateMovementDetectedNotificationArgs(string hardwareId)
    {
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "Movement detected" };
        return args;
    }
}
