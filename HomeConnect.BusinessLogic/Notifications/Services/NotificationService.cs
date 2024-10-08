using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.Notifications.Entities;
using BusinessLogic.Notifications.Models;
using BusinessLogic.Notifications.Repositories;
using BusinessLogic.Users.Entities;

namespace BusinessLogic.Notifications.Services;

public class NotificationService : INotificationService
{
    public NotificationService(INotificationRepository notificationRepository,
        IOwnedDeviceRepository ownedDeviceRepository)
    {
        NotificationRepository = notificationRepository;
        OwnedDeviceRepository = ownedDeviceRepository;
    }

    private INotificationRepository NotificationRepository { get; }
    private IOwnedDeviceRepository OwnedDeviceRepository { get; }

    public void Notify(NotificationArgs args)
    {
        EnsureOwnedDeviceExists(args.HardwareId);
        OwnedDevice ownedDevice = OwnedDeviceRepository.GetByHardwareId(Guid.Parse(args.HardwareId));
        EnsureOwnedDeviceIsNotNull(ownedDevice);
        var home = ownedDevice.Home;
        var shouldReceiveNotification = new HomePermission(HomePermission.GetNotifications);
        NotifyUsersWithPermission(args, home, shouldReceiveNotification, ownedDevice);
    }

    public List<Notification> GetNotifications(Guid userId, string? deviceFilter = null, DateTime? dateFilter = null,
        bool? readFilter = null)
    {
        var notifications = NotificationRepository.Get(userId, deviceFilter, dateFilter, readFilter);
        return notifications;
    }

    public void MarkNotificationsAsRead(List<Notification> notifications)
    {
        throw new NotImplementedException();
    }

    public void CreateNotification(OwnedDevice ownedDevice, string @event, User user)
    {
        var notification = new Notification(Guid.NewGuid(), DateTime.Now, false, @event, ownedDevice, user);
        NotificationRepository.Add(notification);
    }

    private void EnsureOwnedDeviceExists(string argsHardwareId)
    {
        if (!OwnedDeviceRepository.Exists(Guid.Parse(argsHardwareId)))
        {
            throw new KeyNotFoundException("The device is not registered in this home.");
        }
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
            throw new KeyNotFoundException("Device was not found.");
        }
    }
}
