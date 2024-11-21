using System.Globalization;
using BusinessLogic.Notifications.Entities;
using BusinessLogic.Notifications.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using HomeConnect.WebApi.Controllers.Notifications.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Notifications;

[ApiController]
[Route("notifications")]
[AuthenticationFilter]
public sealed class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    [AuthorizationFilter(SystemPermission.GetNotifications)]
    public GetNotificationsResponse GetNotifications([FromQuery] GetNotificationsRequest request)
    {
        var user = HttpContext.Items[Item.UserLogged] as User;
        List<Notification> notifications =
            _notificationService.GetNotifications(request.ToArgs(user!));
        return GetNotificationsResponse.FromNotifications(notifications);
    }
}
