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

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();
        _notificationRepository = new NotificationRepository(_context);
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
        var user = new User("name", "surname", "email@email.com", "Password#100", new Role());
        var device = new Device("Device", 12345, "Device description", "https://example.com/image.png",
            [], "Sensor", new Business("12345", "Business", user));
        var home = new Home(user, "Adress 3420", 100, 100, 5);
        var ownedDevice = new OwnedDevice(home, device);
        var notification = new Notification(Guid.NewGuid(), DateTime.Now, false,
            "Notification message", ownedDevice, user);

        // Act
        _notificationRepository.Add(notification);

        // Assert
        _context.Notifications.Should().Contain(notification);
    }

    #endregion

    #region Error

    [TestMethod]
    public void Add_WhenNotificationExists_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var user = new User("name", "surname", "email@email.com", "Password#100", new Role());
        var device = new Device("Device", 12345, "Device description", "https://example.com/image.png",
            [], "Sensor", new Business("12345", "Business", user));
        var home = new Home(user, "Adress 3420", 100, 100, 5);
        var ownedDevice = new OwnedDevice(home, device);
        var notification = new Notification(Guid.NewGuid(), DateTime.Now, false,
            "Notification message", ownedDevice, user);

        // Act
        _notificationRepository.Add(notification);
        _context.SaveChanges();

        // Assert
        Action act = () => _notificationRepository.Add(notification);
        act.Should().Throw<InvalidOperationException>();
    }

    #endregion

    #endregion
}
