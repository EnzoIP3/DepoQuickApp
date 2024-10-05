using BusinessLogic.Notifications.Entities;

namespace BusinessLogic.Notifications.Repositories;

public interface INotificationRepository
{
    void Add(Notification notification);
    List<Notification> Get(Guid userId, string? deviceFilter, DateTime? dateFilter, bool? readFilter);
}
