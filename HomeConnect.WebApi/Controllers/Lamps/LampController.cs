using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Services;
using BusinessLogic.Notifications.Models;
using BusinessLogic.Notifications.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using HomeConnect.WebApi.Controllers.Devices.Models;
using HomeConnect.WebApi.Controllers.Lamps.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Lamps;

[ApiController]
[Route("lamps")]
public sealed class LampController
    : ControllerBase
{
    private readonly IBusinessOwnerService _businessOwnerService;
    private readonly IDeviceService _deviceService;
    private readonly INotificationService _notificationService;

    public LampController(IBusinessOwnerService businessOwnerService, IDeviceService deviceService,
        INotificationService notificationService)
    {
        _businessOwnerService = businessOwnerService;
        _deviceService = deviceService;
        _notificationService = notificationService;
    }

    [HttpPost]
    [AuthenticationFilter]
    [AuthorizationFilter(SystemPermission.CreateLamp)]
    public CreateLampResponse CreateLamp([FromBody] CreateLampRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        Device createdLamp = _businessOwnerService.CreateDevice(request.ToArgs(userLoggedIn!));
        return new CreateLampResponse { Id = createdLamp.Id };
    }

    [HttpPost("{hardwareId}/turn_on")]
    public NotifyResponse TurnOn([FromRoute] string hardwareId)
    {
        NotificationArgs args = CreateTurnNotificationArgs(hardwareId, true);
        _notificationService.SendLampNotification(args, true);
        _deviceService.TurnLamp(hardwareId, true);
        return new NotifyResponse { HardwareId = hardwareId };
    }

    [HttpPost("{hardwareId}/turn_off")]
    public NotifyResponse TurnOff([FromRoute] string hardwareId)
    {
        NotificationArgs args = CreateTurnNotificationArgs(hardwareId, false);
        _notificationService.SendLampNotification(args, false);
        _deviceService.TurnLamp(hardwareId, false);
        return new NotifyResponse { HardwareId = hardwareId };
    }

    private NotificationArgs CreateTurnNotificationArgs(string hardwareId, bool state)
    {
        return new NotificationArgs
        {
            HardwareId = hardwareId,
            Date = DateTime.Now,
            Event = state ? "Lamp was turned on" : "Lamp was turned off"
        };
    }
}
