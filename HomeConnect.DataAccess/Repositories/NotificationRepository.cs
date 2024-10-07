using BusinessLogic.Devices.Entities;
using BusinessLogic.Notifications.Entities;
using BusinessLogic.Notifications.Repositories;
using Microsoft.EntityFrameworkCore;

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

    public List<Notification> Get(Guid userId, string? deviceFilter = null, DateTime? dateFilter = null,
        bool? readFilter = null)
    {
        DeviceType? deviceTypeFilter = null;
        if (!string.IsNullOrEmpty(deviceFilter))
        {
            if (Enum.TryParse(deviceFilter, out DeviceType deviceType))
            {
                deviceTypeFilter = deviceType;
            }
            else
            {
                throw new ArgumentException("Invalid device type filter");
            }
        }

        return _context.Notifications
            .Include(n => n.OwnedDevice).ThenInclude(od => od.Device)
            .Include(od => od.OwnedDevice).ThenInclude(od => od.Home).ThenInclude(h => h.Owner)
            .Where(n => n.OwnedDevice.Home.Owner.Id == userId ||
                        n.OwnedDevice.Home.Members.Any(m => m.UserId == userId))
            .Where(n => deviceTypeFilter == null || n.OwnedDevice.Device.Type == deviceTypeFilter)
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
