using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.Notifications.Repositories;
using BusinessLogic.Users.Entities;
using HomeConnect.WebApi.Controllers.Sensor;

namespace BusinessLogic.Notifications.Services;

public class NotificationService : INotificationService
{
    private INotificationRepository NotificationRepository { get; init; }
    public NotificationService(INotificationRepository notificationRepository)
    {
        NotificationRepository = notificationRepository;
    }

    public void CreateNotification(OwnedDevice ownedDevice, string @event, User user)
    {
        var notification = new Entities.Notification(Guid.NewGuid(), DateTime.Now, false, @event, ownedDevice, user);
        NotificationRepository.Add(notification);
    }

    public void Notify(NotificationArgs args)
    {
        throw new NotImplementedException();
    }
}
