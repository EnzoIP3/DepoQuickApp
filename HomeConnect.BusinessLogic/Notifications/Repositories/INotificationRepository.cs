using BusinessLogic.Notifications.Entities;

namespace BusinessLogic.Notifications.Repositories;

public interface INotificationRepository
{
    void Add(Notification notification);
    void UpdateRange(List<Notification> notifications);
    List<Notification> Get(Guid userId, string? deviceFilter, DateTime? dateFilter, bool? readFilter);
}
