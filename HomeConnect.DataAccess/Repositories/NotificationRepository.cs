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
        _context.Notifications.Add(notification);
        _context.SaveChanges();
    }

    public List<Notification> GetRange(Guid userId, string? deviceFilter = null, DateTime? dateFilter = null,
        bool? readFilter = null)
    {
        DeviceType? deviceTypeFilter = deviceFilter != null ? Enum.Parse<DeviceType>(deviceFilter) : null;
        return _context.Notifications
            .Include(n => n.User)
            .Include(od => od.OwnedDevice).ThenInclude(od => od.Home).ThenInclude(h => h.Owner)
            .Include(od => od.OwnedDevice).ThenInclude(od => od.Room)
            .Include(od => od.OwnedDevice).ThenInclude(od => od.Device).ThenInclude(d => d.Business)
            .Where(n => n.User!.Id == userId)
            .Where(n => deviceTypeFilter == null || n.OwnedDevice.Device.Type == deviceTypeFilter)
            .Where(n => dateFilter == null || n.Date.Date == dateFilter.Value.Date)
            .Where(n => readFilter == null || n.Read == readFilter)
            .ToList();
    }

    public void UpdateRange(List<Notification> notifications)
    {
        _context.Notifications.UpdateRange(notifications);
        _context.SaveChanges();
    }
}
