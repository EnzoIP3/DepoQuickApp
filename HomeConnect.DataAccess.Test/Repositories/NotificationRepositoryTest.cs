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
    private readonly Context _context = DbContextBuilder.BuildTestDbContext();
    private NotificationRepository _notificationRepository = null!;
    private static readonly Role _role = null!;
    private static User _user = null!;
    private Business _business = null!;
    private Device _sensor = null!;
    private Device _camera = null!;
    private Home _home = null!;
    private OwnedDevice _ownedDevice = null!;
    private OwnedDevice _otherOwnedDevice = null!;
    private Notification _notification = null!;
    private Notification _otherNotification = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();
        _notificationRepository = new NotificationRepository(_context);
        _user = new User("John", "Doe", "email@email.com", "Password#100", _role);
        _business = new Business("123456789123", "Business", _user);
        _sensor = new Device("Device", 12345, "Device description", "https://example.com/image.png",
            [], "Sensor", _business);
        _camera = new Camera("Device", 12345, "Device description", "https://example.com/image.png",
            [], _business, true, true, true, true);
        _home = new Home(_user, "Address 3420", 100, 100, 5);
        _ownedDevice = new OwnedDevice(_home, _sensor);
        _otherOwnedDevice = new OwnedDevice(_home, _camera);
        _notification =
            new Notification(Guid.NewGuid(), DateTime.Now, true, "Notification message", _ownedDevice, _user);
        _otherNotification = new Notification(Guid.NewGuid(), DateTime.Now.AddDays(1), false, "Notification message",
            _otherOwnedDevice, _user);
        _context.Notifications.Add(_notification);
        _context.Notifications.Add(_otherNotification);
        _context.SaveChanges();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    #region Add

    #region Success
    [TestMethod]
    public void Add_WhenNotificationDoesNotExist_ShouldAddNotification()
    {
        // Arrange
        var user = new User("name", "surname", "email2@email.com", "Password#100", new Role());
        var device = new Device("Device", 12345, "Device description", "https://example.com/image.png",
            [], "Sensor", new Business("12345", "Business", user));
        var home = new Home(user, "Address 3420", 100, 100, 5);
        var ownedDevice = new OwnedDevice(home, device);
        var notification = new Notification(Guid.NewGuid(), DateTime.Now, false,
            "Notification message", ownedDevice, user);

        // Act
        _notificationRepository.Add(notification);
        _context.SaveChanges();

        // Assert
        _context.Notifications.Should().Contain(notification);
    }

    #endregion

    #region Error

    [TestMethod]
    public void Add_WhenNotificationExists_ShouldThrowInvalidOperationException()
    {
        // Act
        var act = () => _notificationRepository.Add(_notification);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    #endregion
    #endregion

    #region Get

    [TestMethod]
    public void Get_WhenCalledWithNoFilters_ShouldReturnAllNotifications()
    {
        // Arrange
        var expectedResult = new List<Notification> { _notification, _otherNotification };

        // Act
        var result = _notificationRepository.Get(_user.Id);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public void Get_WhenCalledWithDeviceFilter_ShouldReturnNotificationsForDevice()
    {
        // Arrange
        var expectedResult = new List<Notification> { _notification };

        // Act
        var result = _notificationRepository.Get(_user.Id, _sensor.Type);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public void Get_WhenCalledWithDateFilter_ShouldReturnNotificationsForDate()
    {
        // Arrange
        var expectedResult = new List<Notification> { _notification };

        // Act
        var result = _notificationRepository.Get(_user.Id, dateFilter: DateTime.Now);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public void Get_WhenCalledWithReadFilter_ShouldReturnNotificationsAlreadyRead()
    {
        // Arrange
        var expectedResult = new List<Notification> { _notification };

        // Act
        var result = _notificationRepository.Get(_user.Id, readFilter: true);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
    #endregion
}
