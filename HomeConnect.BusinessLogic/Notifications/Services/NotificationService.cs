using System.Globalization;
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
    private readonly INotificationRepository _notificationRepository;
    private readonly IOwnedDeviceRepository _ownedDeviceRepository;
    private readonly IUserRepository _userRepository;

    public NotificationService(INotificationRepository notificationRepository,
        IOwnedDeviceRepository ownedDeviceRepository, IUserRepository userRepository)
    {
        _notificationRepository = notificationRepository;
        _ownedDeviceRepository = ownedDeviceRepository;
        _userRepository = userRepository;
    }

    public void Notify(NotificationArgs args)
    {
        EnsureUserExists(args.UserEmail);
        EnsureOwnedDeviceExists(args.HardwareId);
        EnsureDeviceIsConnected(args.HardwareId);
        OwnedDevice ownedDevice = _ownedDeviceRepository.GetByHardwareId(Guid.Parse(args.HardwareId));
        Home home = ownedDevice.Home;
        var shouldReceiveNotification = new HomePermission(HomePermission.GetNotifications);
        NotifyUsersWithPermission(args, home, shouldReceiveNotification, ownedDevice);
    }

    public List<Notification> GetNotifications(GetNotificationsArgs args)
    {
        EnsureDeviceFilterIsValid(args.DeviceFilter);
        DateTime? dateFilter = GetDateFromRequest(args.DateFilter);
        List<Notification> notifications =
            _notificationRepository.GetRange(args.UserId, args.DeviceFilter, dateFilter, args.ReadFilter);
        List<Notification> notificationsClone = CloneNotifications(notifications);
        MarkNotificationsAsRead(notifications);
        return notificationsClone;
    }

    public void SendLampNotification(NotificationArgs args, bool state)
    {
        EnsureDeviceIsConnected(args.HardwareId);
        if (_ownedDeviceRepository.GetLampState(Guid.Parse(args.HardwareId)) != state)
        {
            Notify(args);
        }
    }

    public void SendSensorNotification(NotificationArgs args, bool state)
    {
        EnsureDeviceIsSensor(args.HardwareId);
        EnsureDeviceIsConnected(args.HardwareId);
        if (SensorStateChanged(args, state))
        {
            Notify(args);
        }
    }

    private void EnsureUserExists(string? userEmail)
    {
        if (userEmail != null && !_userRepository.ExistsByEmail(userEmail))
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
        OwnedDevice ownedDevice = _ownedDeviceRepository.GetByHardwareId(Guid.Parse(hardwareId));
        return ownedDevice.Connected;
    }

    private static List<Notification> CloneNotifications(List<Notification> notifications)
    {
        return notifications.Select(n => (Notification)n.Clone()).ToList();
    }

    private static DateTime? GetDateFromRequest(string? dateFilter)
    {
        DateTime? dateCreated = null;
        try
        {
            dateCreated = ParseDate(dateFilter, dateCreated);
        }
        catch (FormatException)
        {
            throw new ArgumentException("The date created filter is invalid");
        }

        return dateCreated;
    }

    private static DateTime? ParseDate(string? dateFilter, DateTime? dateCreated)
    {
        if (dateFilter != null)
        {
            dateCreated = DateTime.ParseExact(dateFilter, "dd-MM-yyyy",
                CultureInfo.InvariantCulture);
        }

        return dateCreated;
    }

    private void MarkNotificationsAsRead(List<Notification> notifications)
    {
        notifications.ForEach(notification => notification.Read = true);
        _notificationRepository.UpdateRange(notifications);
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
        _notificationRepository.Add(notification);
    }

    private void EnsureOwnedDeviceExists(string argsHardwareId)
    {
        if (!_ownedDeviceRepository.Exists(Guid.Parse(argsHardwareId)))
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

    private bool SensorStateChanged(NotificationArgs args, bool state)
    {
        return _ownedDeviceRepository.GetSensorState(Guid.Parse(args.HardwareId)) != state;
    }

    private void EnsureDeviceIsSensor(string hardwareId)
    {
        if (_ownedDeviceRepository.GetByHardwareId(Guid.Parse(hardwareId)).Device.Type != DeviceType.Sensor)
        {
            throw new InvalidOperationException("Device is not a sensor.");
        }
    }
}
