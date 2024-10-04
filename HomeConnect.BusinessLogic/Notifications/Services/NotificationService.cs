using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.Notifications.Entities;
using BusinessLogic.Notifications.Repositories;
using BusinessLogic.Users.Entities;
using HomeConnect.WebApi.Controllers.Sensor;

namespace BusinessLogic.Notifications.Services;

public class NotificationService : INotificationService
{
    private INotificationRepository NotificationRepository { get; init; }
    private IOwnedDeviceRepository OwnedDeviceRepository { get; init; }

    public NotificationService(INotificationRepository notificationRepository, IOwnedDeviceRepository ownedDeviceRepository)
    {
        NotificationRepository = notificationRepository;
        OwnedDeviceRepository = ownedDeviceRepository;
    }

    public void CreateNotification(OwnedDevice ownedDevice, string @event, User user)
    {
        var notification = new Entities.Notification(Guid.NewGuid(), DateTime.Now, false, @event, ownedDevice, user);
        NotificationRepository.Add(notification);
    }

    public void Notify(NotificationArgs args)
    {
        var ownedDevice = OwnedDeviceRepository.GetByHardwareId(args.HardwareId);
        EnsureOwnedDeviceIsNotNull(ownedDevice);
        var home = ownedDevice.Home;
        var shouldReceiveNotification = new HomePermission("shouldBeNotified");
        NotifyUsersWithPermission(args, home, shouldReceiveNotification, ownedDevice);
    }

    public List<Notification> GetNotifications(Guid userId, string? deviceFilter = null, DateTime? dateFilter = null,
        bool? readFilter = null)
    {
        return NotificationRepository.GetNotifications(userId, deviceFilter, dateFilter, readFilter);
    }

    private void NotifyUsersWithPermission(NotificationArgs args, Home home, HomePermission shouldReceiveNotification,
        OwnedDevice ownedDevice)
    {
        var usersToNotify = new List<User> { home.Owner };
        usersToNotify.AddRange(home.Members
            .Where(member => member.HasPermission(shouldReceiveNotification))
            .Select(member => member.User));
        usersToNotify.ForEach(user => CreateNotification(ownedDevice, args.Event, user));
    }

    private static void EnsureOwnedDeviceIsNotNull(OwnedDevice ownedDevice)
    {
        if (ownedDevice == null)
        {
            throw new ArgumentException("Device not found");
        }
    }
}
