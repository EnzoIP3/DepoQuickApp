using BusinessLogic.Notifications.Repositories;
using BusinessLogic.Notifications.Services;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HomeConnect.WebApi.Controllers.Notification;

[ApiController]
[Route("notifications")]
[AuthorizationFilter]
public class NotificationController() : ControllerBase
{
    [HttpGet]
    public GetNotificationsResponse GetNotifications(GetNotificationsRequest request, AuthorizationFilterContext context)
    {
        throw new NotImplementedException();
    }
}
