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

    public List<Notification> Get(Guid userId, string? deviceFilter = null, DateTime? dateFilter = null, bool? readFilter = null)
    {
        return _context.Notifications
            .Where(n => n.OwnedDevice.Home.Owner.Id == userId)
            .Where(n => deviceFilter == null || n.OwnedDevice.Device.Type == deviceFilter)
            .Where(n => dateFilter == null || n.Date.Date == dateFilter.Value.Date)
            .Where(n => readFilter == null || n.Read == readFilter)
            .ToList();
    }

    private void EnsureNotificationDoesNotExist(Notification notification)
    {
        if (_context.Notifications.Any(n => n.Id == notification.Id))
        {
            throw new InvalidOperationException("Notification already exists.");
        }
    }
}
