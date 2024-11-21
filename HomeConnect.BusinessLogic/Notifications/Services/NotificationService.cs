using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.Notifications.Entities;
using BusinessLogic.Notifications.Models;
using BusinessLogic.Notifications.Repositories;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Repositories;

namespace BusinessLogic.Notifications.Services;

public class NotificationService : INotificationService
{
    public NotificationService(INotificationRepository notificationRepository,
        IOwnedDeviceRepository ownedDeviceRepository, IUserRepository userRepository)
    {
        NotificationRepository = notificationRepository;
        OwnedDeviceRepository = ownedDeviceRepository;
        UserRepository = userRepository;
    }

    private INotificationRepository NotificationRepository { get; }
    private IOwnedDeviceRepository OwnedDeviceRepository { get; }
    private IUserRepository UserRepository { get; }

    public void Notify(NotificationArgs args)
    {
        EnsureUserExists(args.UserEmail);
        EnsureOwnedDeviceExists(args.HardwareId);
        EnsureDeviceIsConnected(args.HardwareId);
        OwnedDevice ownedDevice = OwnedDeviceRepository.GetByHardwareId(Guid.Parse(args.HardwareId));
        Home home = ownedDevice.Home;
        var shouldReceiveNotification = new HomePermission(HomePermission.GetNotifications);
        NotifyUsersWithPermission(args, home, shouldReceiveNotification, ownedDevice);
    }

    private void EnsureUserExists(string? userEmail)
    {
        if (userEmail != null && !UserRepository.ExistsByEmail(userEmail))
        {
            throw new ArgumentException("User detected by camera was not found.");
        }
    }

    private void EnsureDeviceIsConnected(string hardwareId)
    {
        if (!IsConnected(hardwareId))
        {
            throw new ArgumentException("Device is not connected");
        }
    }

    private bool IsConnected(string hardwareId)
    {
        OwnedDevice ownedDevice = OwnedDeviceRepository.GetByHardwareId(Guid.Parse(hardwareId));
        return ownedDevice.Connected;
    }

    public List<Notification> GetNotifications(Guid userId, string? deviceFilter = null, DateTime? dateFilter = null,
        bool? readFilter = null)
    {
        EnsureDeviceFilterIsValid(deviceFilter);
        List<Notification> notifications =
            NotificationRepository.GetRange(userId, deviceFilter, dateFilter, readFilter);
        return notifications;
    }

    public void MarkNotificationsAsRead(List<Notification> notifications)
    {
        notifications.ForEach(notification => notification.Read = true);
        NotificationRepository.UpdateRange(notifications);
    }

    private void EnsureDeviceFilterIsValid(string? deviceFilter)
    {
        if (deviceFilter != null && !Enum.TryParse<DeviceType>(deviceFilter, out _))
        {
            throw new ArgumentException("The device filter is not valid.");
        }
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

    public void SendLampNotification(NotificationArgs args, bool state)
    {
        if (OwnedDeviceRepository.GetLampState(Guid.Parse(args.HardwareId)) != state)
        {
            Notify(args);
        }
    }

    public void SendSensorNotification(NotificationArgs args, bool state)
    {
        if (OwnedDeviceRepository.GetSensorState(Guid.Parse(args.HardwareId)) != state)
        {
            Notify(args);
        }
    }
}
