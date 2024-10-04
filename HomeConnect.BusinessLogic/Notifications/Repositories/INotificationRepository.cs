using BusinessLogic.Notifications.Entities;

namespace BusinessLogic.Notifications.Repositories;

public interface INotificationRepository
{
    void Add(Entities.Notification notification);
    List<Notification> GetNotifications(Guid userId, string? deviceFilter, DateTime? dateFilter, bool? readFilter);
}
