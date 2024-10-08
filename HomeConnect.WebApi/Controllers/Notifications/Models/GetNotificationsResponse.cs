namespace HomeConnect.WebApi.Controllers.Notifications.Models;

public record GetNotificationsResponse
{
    public List<NotificationData> Notifications { get; set; } = null!;
}
