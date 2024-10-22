using BusinessLogic.Devices.Services;
using BusinessLogic.Notifications.Entities;
using BusinessLogic.Notifications.Models;

namespace BusinessLogic.Notifications.Services;

public interface INotificationService
{
    void Notify(NotificationArgs args, IDeviceService deviceService);
    List<Notification> GetNotifications(Guid userId, string? deviceFilter, DateTime? dateFilter, bool? readFilter);
    void MarkNotificationsAsRead(List<Notification> notifications);
}
