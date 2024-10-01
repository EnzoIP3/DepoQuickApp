namespace BusinessLogic;

public class NotificationService
{
    private INotificationRepository NotificationRepository { get; init; }

    public NotificationService(INotificationRepository notificationRepository)
    {
        NotificationRepository = notificationRepository;
    }

    public void CreateNotification(OwnedDevice ownedDevice, string @event, User user)
    {
        var notification = new Notification(Guid.NewGuid(), DateTime.Now, false, @event, ownedDevice, user);
        NotificationRepository.Add(notification);
    }
}
