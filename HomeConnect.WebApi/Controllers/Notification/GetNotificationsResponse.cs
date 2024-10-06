namespace HomeConnect.WebApi.Controllers.Notification;

public record GetNotificationsResponse
{
    public List<NotificationData> Notifications { get; set; } = null!;
}
