using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.Devices.Services;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.Notifications.Entities;
using BusinessLogic.Notifications.Models;
using BusinessLogic.Notifications.Repositories;
using BusinessLogic.Notifications.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Repositories;
using FluentAssertions;
using Moq;

namespace HomeConnect.BusinessLogic.Test.Notifications.Services;

[TestClass]
public class NotificationServiceTest
{
    private static readonly Role _role = new() { Name = "HomeOwner", Permissions = [] };
    private readonly string _modelNumber = "12345";
    private readonly User _user = new("owner", "owner", "owner@email.com", "Password@100", _role);
    private Mock<INotificationRepository> _mockNotificationRepository = null!;
    private Mock<IOwnedDeviceRepository> _mockOwnedDeviceRepository = null!;
    private Mock<IUserRepository> _mockUserRepository = null!;
    private NotificationService _notificationService = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockOwnedDeviceRepository = new Mock<IOwnedDeviceRepository>(MockBehavior.Strict);
        _mockNotificationRepository = new Mock<INotificationRepository>(MockBehavior.Strict);
        _mockUserRepository = new Mock<IUserRepository>(MockBehavior.Strict);
        _notificationService =
            new NotificationService(_mockNotificationRepository.Object, _mockOwnedDeviceRepository.Object,
                _mockUserRepository.Object);
    }

    #region CreateNotification

    [TestMethod]
    public void CreateNotification_WhenCalled_AddsNotificationToRepository()
    {
        // Arrange
        var user = new User("name", "surname", "email@email.com", "Password#100",
            new Role());
        var device = new Device("Device", _modelNumber, "Device description",
            "https://example.com/image.png",
            [], "Sensor", new Business("Rut", "Business", "https://example.com/image.png", user));
        var home = new Home(user, "Adress 3420", 50, 100, 5);
        var ownedDevice = new OwnedDevice(home, device);
        const string @event = "Test Event";
        _mockNotificationRepository.Setup(x => x.Add(It.IsAny<Notification>())).Verifiable();

        // Act
        _notificationService.CreateNotification(ownedDevice, @event, user);

        // Assert
        _mockNotificationRepository.Verify(x => x.Add(It.Is<Notification>(
            n =>
                n.Event == @event &&
                n.User == user &&
                n.OwnedDevice == ownedDevice)), Times.Once);
    }

    #endregion

    #region MarkNotificationsAsRead

    [TestMethod]
    public void MarkNotificationsAsRead_WhenCalledWithNotifications_ShouldMarkNotificationsAsRead()
    {
        // Arrange
        var notifications = new List<Notification>
        {
            new(Guid.NewGuid(), DateTime.Now, false, "Test Event", new OwnedDevice(
                    new Home(_user, "Street 3420", 50, 100, 5),
                    new Device("Device", _modelNumber, "Device description", "https://example.com/image.png", [],
                        "Sensor",
                        new Business())),
                new User("name", "surname", "email@email.com", "Password@100", new Role())),
            new(Guid.NewGuid(), DateTime.Now, false, "Test Event", new OwnedDevice(
                    new Home(_user, "Street 3420", 50, 100, 5),
                    new Device("Device", _modelNumber, "Device description", "https://example.com/image.png", [],
                        "Sensor",
                        new Business())),
                new User("name2", "surname2", "email2@email.com", "Password@100", new Role()))
        };
        _mockNotificationRepository.Setup(x => x.UpdateRange(notifications)).Verifiable();

        // Act
        _notificationService.MarkNotificationsAsRead(notifications);

        // Assert
        _mockNotificationRepository.VerifyAll();
        notifications.ForEach(n => n.Read.Should().BeTrue());
    }

    #endregion

    #region GetNotifications

    [TestMethod]
    public void GetNotifications_WhenCalled_ReturnsNotifications()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var deviceFilter = "Sensor";
        DateTime dateFilter = DateTime.Now;
        var readFilter = true;
        var notifications = new List<Notification>
        {
            new(Guid.NewGuid(), DateTime.Now, false, "Test Event", new OwnedDevice(
                    new Home(_user, "Street 3420", 50, 100, 5),
                    new Device("Device", _modelNumber, "Device description", "https://example.com/image.png", [],
                        "Sensor",
                        new Business())),
                new User("name", "surname", "email@email.com", "Password@100", new Role())),
            new(Guid.NewGuid(), DateTime.Now, false, "Test Event", new OwnedDevice(
                    new Home(_user, "Street 3420", 50, 100, 5),
                    new Device("Device", _modelNumber, "Device description", "https://example.com/image.png", [],
                        "Sensor",
                        new Business())),
                new User("name2", "surname2", "email2@email.com", "Password@100", new Role()))
        };
        _mockNotificationRepository.Setup(x => x.GetRange(userId, deviceFilter, dateFilter, readFilter))
            .Returns(notifications);

        // Act
        List<Notification> result = _notificationService.GetNotifications(userId, deviceFilter, dateFilter, readFilter);

        // Assert
        _mockNotificationRepository.VerifyAll();
        result.Should().BeEquivalentTo(notifications);
    }

    [TestMethod]
    public void GetNotifications_WhenDeviceFilterIsInvalid_ThrowsException()
    {
        // Arrange
        var deviceFilter = "Device";

        // Act
        Func<List<Notification>> act = () => _notificationService.GetNotifications(Guid.NewGuid(), deviceFilter);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #region Notify

    [TestMethod]
    public void Notify_WhenCalledWithNonExistentOwnedDevice_ThrowsException()
    {
        // Arrange
        var args = new NotificationArgs
        {
            HardwareId = Guid.NewGuid().ToString(),
            Event = "Test Event",
            Date = DateTime.Now
        };

        _mockOwnedDeviceRepository.Setup(x => x.Exists(Guid.Parse(args.HardwareId))).Returns(false);

        // Act
        Action act = () => _notificationService.Notify(args);

        // Assert
        act.Should().Throw<KeyNotFoundException>();
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
        var device = new Device("Device", _modelNumber, "Device description", "https://example.com/image.png",
            [], "Sensor", new Business("Rut", "Business", "https://example.com/image.png", owner));
        var ownedDevice = new OwnedDevice(home, device);
        var args = new NotificationArgs
        {
            HardwareId = ownedDevice.HardwareId.ToString(),
            Event = "Test Event",
            Date = DateTime.Now
        };
        _mockOwnedDeviceRepository.Setup(x => x.Exists(Guid.Parse(args.HardwareId))).Returns(true);
        _mockOwnedDeviceRepository.Setup(x => x.GetByHardwareId(Guid.Parse(args.HardwareId))).Returns(ownedDevice);
        _mockNotificationRepository.Setup(x => x.Add(It.IsAny<Notification>())).Verifiable();

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

    [TestMethod]
    public void Notify_WhenDeviceIsDisconnected_ThrowsArgumentException()
    {
        // Arrange
        var ownedDevice = new OwnedDevice(new Home(_user, "Street 3420", 50, 100, 5),
            new Device("Device", _modelNumber, "Device description", "https://example.com/image.png", [],
                "Sensor", new Business()));
        ownedDevice.Connected = false;
        var hardwareId = ownedDevice.HardwareId.ToString();
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "example" };
        _mockUserRepository.Setup(x => x.ExistsByEmail(_user.Email)).Returns(true);
        _mockOwnedDeviceRepository.Setup(x => x.Exists(Guid.Parse(hardwareId))).Returns(true);
        _mockOwnedDeviceRepository.Setup(x => x.GetByHardwareId(Guid.Parse(hardwareId))).Returns(ownedDevice);

        // Act
        var act = () => _notificationService.Notify(args);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Device is not connected");
    }

    [TestMethod]
    public void Notify_WhenUserDoesNotExist_ThrowsArgumentException()
    {
        // Arrange
        var userEmail = "email";
        var args = new NotificationArgs { UserEmail = userEmail, Date = DateTime.Now, Event = "example" };
        _mockUserRepository.Setup(x => x.ExistsByEmail(userEmail)).Returns(false);

        // Act
        var act = () => _notificationService.Notify(args);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("User detected by camera was not found.");
        _mockUserRepository.VerifyAll();
    }

    #endregion

    #region SendLampNotification

    [TestMethod]
    public void SendLampNotification_IfStateDiffersFromLampState_SendsNotification()
    {
        // Arrange
        var hardwareId = Guid.NewGuid().ToString();
        var date = DateTime.Now;
        var state = true;
        var args = new NotificationArgs { HardwareId = hardwareId, Date = date, Event = "Lamp was turned on" };
        var ownedDevice = new OwnedDevice(new Home(_user, "Street 3420", 50, 100, 5),
            new Device("Device", _modelNumber, "Device description", "https://example.com/image.png", [],
                "Sensor", new Business()));
        _mockOwnedDeviceRepository.Setup(x => x.Exists(Guid.Parse(hardwareId))).Returns(true);
        _mockOwnedDeviceRepository.Setup(x => x.GetByHardwareId(Guid.Parse(hardwareId))).Returns(ownedDevice);
        _mockNotificationRepository.Setup(x => x.Add(It.IsAny<Notification>())).Verifiable();
        _mockOwnedDeviceRepository.Setup(x => x.GetLampState(Guid.Parse(hardwareId))).Returns(!state);

        // Act
        _notificationService.SendLampNotification(args, state);

        // Assert
        _mockNotificationRepository.Verify(x => x.Add(It.Is<Notification>(
            n =>
                n.Event == args.Event &&
                n.User == _user &&
                n.OwnedDevice == ownedDevice)), Times.Once);
    }
    #endregion
}
