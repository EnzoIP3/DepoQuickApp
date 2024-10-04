using BusinessLogic.Notifications.Entities;
using BusinessLogic.Notifications.Repositories;

namespace HomeConnect.DataAccess.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly Context _context;

    public NotificationRepository(Context context)
    {
        _context = context;
    }

    public void Add(Notification notification)
    {
        EnsureNotificationDoesNotExist(notification);
        _context.Notifications.Add(notification);
        _context.SaveChanges();
    }

    public List<Notification> Get(Guid userId, string? deviceFilter = null, DateTime? dateFilter= null, bool? readFilter= null)
    {
        throw new NotImplementedException();
    }

    private void EnsureNotificationDoesNotExist(Notification notification)
    {
        if (_context.Notifications.Any(n => n.Id == notification.Id))
        {
            throw new InvalidOperationException("Notification already exists.");
        }
    }
}
