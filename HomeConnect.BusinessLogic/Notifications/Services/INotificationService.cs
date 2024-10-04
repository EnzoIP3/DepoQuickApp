using BusinessLogic.Notifications.Entities;
using HomeConnect.WebApi.Controllers.Sensor;

namespace BusinessLogic.Notifications.Services;

public interface INotificationService
{
    void Notify(NotificationArgs args);
    List<Notification> GetNotifications(Guid userId, string? deviceFilter, DateTime? dateFilter, bool? readFilter);
}
