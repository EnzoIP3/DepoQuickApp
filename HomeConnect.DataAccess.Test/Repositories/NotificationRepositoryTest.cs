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
    private static Role _role = null!;
    private static User _user = null!;
    private Business _business = null!;
    private Device _device = null!;
    private Home _home = null!;
    private OwnedDevice _ownedDevice = null!;
    private Notification _notification = null!;
    private Notification _otherNotification = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();
        _notificationRepository = new NotificationRepository(_context);
        _user = new User("John", "Doe", "email@email.com", "Password#100", _role);
        _business = new Business("123456789123", "Business", _user);
        _device = new Device("Device", 12345, "Device description", "https://example.com/image.png",
            [], "Sensor", _business);
        _home = new Home(_user, "Address 3420", 100, 100, 5);
        _ownedDevice = new OwnedDevice(_home, _device);
        _notification =
            new Notification(Guid.NewGuid(), DateTime.Now, true, "Notification message", _ownedDevice, _user);
        _otherNotification = new Notification(Guid.NewGuid(), DateTime.Now.AddDays(1), false, "Notification message",
            _ownedDevice, _user);
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

    [TestMethod]
    public void Add_WhenNotificationExists_ShouldThrowInvalidOperationException()
    {
        // Act
        var act = () => _notificationRepository.Add(_notification);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    #endregion
}
