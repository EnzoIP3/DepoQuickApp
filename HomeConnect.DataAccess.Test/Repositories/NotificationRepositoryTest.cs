using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.Notifications.Entities;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.DataAccess.Repositories;

namespace HomeConnect.DataAccess.Test.Repositories;

[TestClass]
public class NotificationRepositoryTest
{
    private static User _user = null!;
    private readonly Context _context = DbContextBuilder.BuildTestDbContext();
    private Business _business = null!;
    private Device _camera = null!;
    private Home _home = null!;
    private Notification _notification = null!;
    private NotificationRepository _notificationRepository = null!;
    private Notification _otherNotification = null!;
    private OwnedDevice _otherOwnedDevice = null!;
    private OwnedDevice _ownedDevice = null!;
    public Role _role = new("Role", []);
    private Device _sensor = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();
        _notificationRepository = new NotificationRepository(_context);
        _context.Roles.Add(_role);
        _context.SaveChanges();

        _user = new User("John", "Doe", "email@email.com", "Password#100", _role);
        _context.Users.Add(_user);
        _context.SaveChanges();

        _business = new Business("123456789123", "Business", "https://example.com/image.png", _user);
        _context.Businesses.Add(_business);
        _context.SaveChanges();
        _sensor = new Device("Device", "12345", "Device description", "https://example.com/image.png", [],
            "Sensor", _business);
        _camera = new Camera("Device", "12345", "Device description", "https://example.com/image.png", [],
            _business, true, true, true, true);
        _context.Devices.AddRange(_sensor, _camera);
        _context.SaveChanges();

        _home = new Home(_user, "Address 3420", 50, 100, 5);
        _context.Homes.Add(_home);
        _context.SaveChanges();

        _ownedDevice = new OwnedDevice(_home, _sensor);
        _otherOwnedDevice = new OwnedDevice(_home, _camera);
        _context.OwnedDevices.AddRange(_ownedDevice, _otherOwnedDevice);
        _context.SaveChanges();

        _notification =
            new Notification(Guid.NewGuid(), DateTime.Now, true, "Notification message", _ownedDevice, _user);
        _otherNotification = new Notification(Guid.NewGuid(), DateTime.Now.AddDays(1), false, "Notification message",
            _otherOwnedDevice, _user);
        _context.Notifications.AddRange(_notification, _otherNotification);
        _context.SaveChanges();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    #region UpdateRange

    [TestMethod]
    public void UpdateRange_WhenCalled_UpdatesNotifications()
    {
        // Arrange
        var notifications = new List<Notification> { _notification, _otherNotification };
        notifications.ForEach(n => n.Read = true);

        // Act
        _notificationRepository.UpdateRange(notifications);

        // Assert
        _context.Notifications.Should().BeEquivalentTo(notifications);
    }

    #endregion

    #region Add

    #region Success

    [TestMethod]
    public void Add_WhenNotificationDoesNotExist_AddNotification()
    {
        // Arrange
        var user = new User("name", "surname", "email2@email.com", "Password#100", new Role());
        var device = new Device("Device", "12345", "Device description", "https://example.com/image.png",
            [], "Sensor", new Business("12345", "Business", "https://example.com/image.png", user));
        var home = new Home(user, "Address 3420", 50, 100, 5);
        var ownedDevice = new OwnedDevice(home, device);
        var notification = new Notification(Guid.NewGuid(), DateTime.Now, false,
            "Notification message", ownedDevice, user);

        // Act
        _notificationRepository.Add(notification);

        // Assert
        _context.Notifications.Should().Contain(notification);
    }

    #endregion

    #endregion

    #region Get

    [TestMethod]
    public void Get_WhenCalledWithNoFilters_ReturnsAllNotifications()
    {
        // Arrange
        var expectedResult = new List<Notification> { _notification, _otherNotification };

        // Act
        List<Notification> result = _notificationRepository.GetRange(_user.Id);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public void Get_WhenCalledWithDeviceFilter_ReturnsNotificationsForDevice()
    {
        // Arrange
        var expectedResult = new List<Notification> { _notification };

        // Act
        List<Notification> result = _notificationRepository.GetRange(_user.Id, _sensor.Type.ToString());

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public void Get_WhenCalledWithDateFilter_ReturnsNotificationsForDate()
    {
        // Arrange
        var expectedResult = new List<Notification> { _notification };

        // Act
        List<Notification> result = _notificationRepository.GetRange(_user.Id, dateFilter: DateTime.Now);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public void Get_WhenCalledWithReadFilter_ReturnsNotificationsAlreadyRead()
    {
        // Arrange
        var expectedResult = new List<Notification> { _notification };

        // Act
        List<Notification> result = _notificationRepository.GetRange(_user.Id, readFilter: true);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    #endregion
}
