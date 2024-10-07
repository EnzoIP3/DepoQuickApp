using BusinessLogic.Notifications.Services;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HomeConnect.WebApi.Controllers.Notification;

[ApiController]
[Route("notifications")]
[AuthenticationFilter]
public class NotificationController(INotificationService notificationService) : ControllerBase
{
    [HttpGet]
    public GetNotificationsResponse GetNotifications([FromQuery] GetNotificationsRequest request,
        AuthorizationFilterContext context)
    {
        var user = (BusinessLogic.Users.Entities.User)context.HttpContext.Items[Item.UserLogged];
        List<BusinessLogic.Notifications.Entities.Notification> notifications =
            notificationService.GetNotifications(user.Id, request.Device, request.DateCreated, request.Read);
        var response = new GetNotificationsResponse
        {
            Notifications = notifications.Select(n => new NotificationData
            {
                Event = n.Event,
                DeviceId = n.OwnedDevice.HardwareId.ToString(),
                Read = n.Read,
                DateCreated = n.Date
            }).ToList()
        };
        return response;
    }
}
