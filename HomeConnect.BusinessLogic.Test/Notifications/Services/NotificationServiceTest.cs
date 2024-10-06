using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.Notifications.Entities;
using BusinessLogic.Notifications.Models;
using BusinessLogic.Notifications.Repositories;
using BusinessLogic.Notifications.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using Moq;

namespace HomeConnect.BusinessLogic.Test.Notifications.Services;

[TestClass]

public class NotificationServiceTest
{
    private Mock<INotificationRepository> _mockNotificationRepository = null!;
    private Mock<IOwnedDeviceRepository> _mockOwnedDeviceRepository = null!;
    private NotificationService _notificationService = null!;
    private static readonly Role _role = new Role { Name = "HomeOwner", Permissions = [] };
    private readonly User _user = new User("owner", "owner", "owner@email.com", "Password@100", _role);

    [TestInitialize]
    public void TestInitialize()
    {
        _mockOwnedDeviceRepository = new Mock<IOwnedDeviceRepository>();
        _mockNotificationRepository = new Mock<INotificationRepository>();
        _notificationService =
            new NotificationService(_mockNotificationRepository.Object, _mockOwnedDeviceRepository.Object);
    }

    #region CreateNotification

    [TestMethod]
    public void CreateNotification_WhenCalled_AddsNotificationToRepository()
    {
        // Arrange
        var user = new global::BusinessLogic.Users.Entities.User("name", "surname", "email@email.com", "Password#100",
            new global::BusinessLogic.Roles.Entities.Role());
        var device = new global::BusinessLogic.Devices.Entities.Device("Device", 12345, "Device description",
            "https://example.com/image.png",
            [], "Sensor", new Business("Rut", "Business", "https://example.com/image.png", user));
        var home = new Home(user, "Adress 3420", 50, 100, 5);
        var ownedDevice = new OwnedDevice(home, device);
        const string @event = "Test Event";

        // Act
        _notificationService.CreateNotification(ownedDevice, @event, user);

        // Assert
        _mockNotificationRepository.Verify(x => x.Add(It.Is<global::BusinessLogic.Notifications.Entities.Notification>(
            n =>
                n.Event == @event &&
                n.User == user &&
                n.OwnedDevice == ownedDevice)), Times.Once);
    }

    #endregion

    #region Notify

    [TestMethod]
    public void Notify_WhenCalledWithNonExistentOwnedDevice_ShouldThrowArgumentException()
    {
        // Arrange
        var args = new NotificationArgs
        {
            HardwareId = Guid.NewGuid().ToString(),
            Event = "Test Event",
            Date = DateTime.Now
        };

        _mockOwnedDeviceRepository.Setup(x => x.Exists(args.HardwareId)).Returns(false);

        // Act
        var act = () => _notificationService.Notify(args);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Notify_WhenCalledWithExistentDevice_ShouldCreateNotificationForEachUserWithPermissions()
    {
        // Arrange
        var shouldBeNotified = new HomePermission(HomePermission.GetNotifications);
        var owner = new User("owner", "owner", "owner@email.com", "Password@100",
            _role);
        var member = new Member(_user, [shouldBeNotified]);
        var otherMember = new Member(new User("name", "surname", "email@email.com", "Password@100",
            _role));
        var home = new Home(owner, "Street 3420", 50, 100, 5);
        home.AddMember(member);
        home.AddMember(otherMember);
        var device = new Device("Device", 12345, "Device description", "https://example.com/image.png",
            [], "Sensor", new Business("Rut", "Business", "https://example.com/image.png", owner));
        var ownedDevice = new OwnedDevice(home, device);
        var args = new NotificationArgs
        {
            HardwareId = ownedDevice.HardwareId.ToString(),
            Event = "Test Event",
            Date = DateTime.Now
        };
        _mockOwnedDeviceRepository.Setup(x => x.Exists(args.HardwareId)).Returns(true);
        _mockOwnedDeviceRepository.Setup(x => x.GetByHardwareId(args.HardwareId)).Returns(ownedDevice);

        // Act
        _notificationService.Notify(args);

        // Assert
        _mockNotificationRepository.Verify(x => x.Add(It.Is<Notification>(
            n =>
                n.Event == args.Event &&
                n.OwnedDevice == ownedDevice &&
                n.User == owner)), Times.Once);
        _mockNotificationRepository.Verify(x => x.Add(It.Is<Notification>(
            n =>
                n.Event == args.Event &&
                n.OwnedDevice == ownedDevice &&
                n.User == member.User)), Times.Once);
    }

    #endregion

    #region GetNotifications

    [TestMethod]
    public void GetNotifications_WhenCalled_ReturnsNotifications()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var deviceFilter = "Device";
        var dateFilter = DateTime.Now;
        var readFilter = true;
        var notifications = new List<Notification>
        {
            new Notification(Guid.NewGuid(), DateTime.Now, false, "Test Event", new OwnedDevice(
                    new Home(_user, "Street 3420", 50, 100, 5),
                    new Device("Device", 12345, "Device description", "https://example.com/image.png", [], "Sensor",
                        new Business())),
                new User("name", "surname", "email@email.com", "Password@100", new Role())),
            new Notification(Guid.NewGuid(), DateTime.Now, false, "Test Event", new OwnedDevice(
                    new Home(_user, "Street 3420", 50, 100, 5),
                    new Device("Device", 12345, "Device description", "https://example.com/image.png", [], "Sensor",
                        new Business())),
                new User("name2", "surname2", "email2@email.com", "Password@100", new Role()))
        };
        _mockNotificationRepository.Setup(x => x.Get(userId, deviceFilter, dateFilter, readFilter))
            .Returns(notifications);

        // Act
        var result = _notificationService.GetNotifications(userId, deviceFilter, dateFilter, readFilter);

        // Assert
        _mockNotificationRepository.VerifyAll();
        result.Should().BeEquivalentTo(notifications);
    }
    #endregion
}
