using System.Globalization;
using BusinessLogic.Notifications.Entities;

namespace HomeConnect.WebApi.Controllers.Notifications.Models;

public record GetNotificationsResponse
{
    public List<NotificationData> Notifications { get; set; } = null!;

    public static GetNotificationsResponse FromNotifications(List<Notification> notifications)
    {
        return new GetNotificationsResponse
        {
            Notifications = notifications.Select(n => new NotificationData
            {
                Event = n.Event,
                Device = n.OwnedDevice.ToOwnedDeviceDto(),
                Read = n.Read,
                DateCreated = n.Date.ToString("dd MMMM yyyy HH:mm:ss", CultureInfo.InvariantCulture)
            }).ToList()
        };
    }
}
