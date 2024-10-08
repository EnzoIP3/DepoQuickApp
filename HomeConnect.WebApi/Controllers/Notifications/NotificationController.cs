using System.Globalization;
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
    public GetNotificationsResponse GetNotifications([FromQuery] GetNotificationsRequest request)
    {
        DateTime? dateCreated = GetDateFromRequest(request);
        var user = HttpContext.Items[Item.UserLogged] as BusinessLogic.Users.Entities.User;
        List<BusinessLogic.Notifications.Entities.Notification> notifications =
            notificationService.GetNotifications(user!.Id, request.Device, dateCreated, request.Read);
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
