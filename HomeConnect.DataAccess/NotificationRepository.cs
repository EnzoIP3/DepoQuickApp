using BusinessLogic;

namespace HomeConnect.DataAccess;

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
}
