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
public class NotificationController(INotificationService notificationService) : ControllerBase
{
    [HttpGet]
    [AuthorizationFilter(SystemPermission.GetNotifications)]
    public GetNotificationsResponse GetNotifications([FromQuery] GetNotificationsRequest request)
    {
        DateTime? dateCreated = GetDateFromRequest(request);
        var user = HttpContext.Items[Item.UserLogged] as User;
        List<Notification> notifications =
            notificationService.GetNotifications(user!.Id, request.Device, dateCreated, request.Read);
        var response = new GetNotificationsResponse
        {
            Notifications = notifications.Select(n => new NotificationData
            {
                Event = n.Event,
                DeviceId = n.OwnedDevice.HardwareId.ToString(),
                Read = n.Read,
                DateCreated = n.Date.ToString("dd MMMM yyyy HH:mm:ss")
            }).ToList()
        };
        notificationService.MarkNotificationsAsRead(notifications);
        return response;
    }

    private static DateTime? GetDateFromRequest(GetNotificationsRequest request)
    {
        DateTime? dateCreated = null;
        try
        {
            dateCreated = ParseDateCreated(request, dateCreated);
        }
        catch (FormatException)
        {
            throw new ArgumentException("The date created filter is invalid");
        }

        return dateCreated;
    }

    private static DateTime? ParseDateCreated(GetNotificationsRequest request, DateTime? dateCreated)
    {
        if (request.DateCreated != null)
        {
            dateCreated = DateTime.ParseExact(request.DateCreated, "dd-MM-yyyy",
                CultureInfo.InvariantCulture);
        }

        return dateCreated;
    }
}
