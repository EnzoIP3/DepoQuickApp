using BusinessLogic.Notifications.Entities;
using BusinessLogic.Notifications.Models;

namespace BusinessLogic.Notifications.Services;

public interface INotificationService
{
    void Notify(NotificationArgs args);
    List<Notification> GetNotifications(GetNotificationsArgs args);
    void SendLampNotification(NotificationArgs args, bool state);
    void SendSensorNotification(NotificationArgs notificationArgs, bool state);
}
