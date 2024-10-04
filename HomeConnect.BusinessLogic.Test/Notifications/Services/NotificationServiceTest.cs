using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.Notifications.Repositories;
using BusinessLogic.Notifications.Services;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Sensor;
using Moq;

namespace HomeConnect.BusinessLogic.Test.Notifications.Services;

[TestClass]

public class NotificationServiceTest
{
    private Mock<INotificationRepository> _mockNotificationRepository = null!;
    private Mock<IOwnedDeviceRepository> _mockOwnedDeviceRepository = null!;
    private NotificationService _notificationService = null!;
    [TestInitialize]
    public void TestInitialize()
    {
        _mockOwnedDeviceRepository = new Mock<IOwnedDeviceRepository>();
        _mockNotificationRepository = new Mock<INotificationRepository>();
        _notificationService = new NotificationService(_mockNotificationRepository.Object, _mockOwnedDeviceRepository.Object);
    }

    [TestMethod]
    public void CreateNotification_WhenCalled_AddsNotificationToRepository()
    {
        // Arrange
        var user = new global::BusinessLogic.Users.Entities.User("name", "surname", "email@email.com", "Password#100", new global::BusinessLogic.Roles.Entities.Role());
        var device = new global::BusinessLogic.Devices.Entities.Device("Device", 12345, "Device description", "https://example.com/image.png",
            [], "Sensor", new Business("Rut", "Business", user));
        var home = new Home(user, "Adress 3420", 100, 100, 5);
        var ownedDevice = new OwnedDevice(home, device);
        const string @event = "Test Event";

        // Act
        _notificationService.CreateNotification(ownedDevice, @event, user);

        // Assert
        _mockNotificationRepository.Verify(x => x.Add(It.Is<global::BusinessLogic.Notifications.Entities.Notification>(n =>
            n.Event == @event &&
            n.User == user &&
            n.OwnedDevice == ownedDevice)), Times.Once);
    }

    [TestMethod]
    public void Notify_WhenCalledWithNonExistentDevice_ShouldThrowArgumentException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var args = new NotificationArgs { HardwareId = id.ToString(), Event = "Test Event", Date = DateTime.Now };
        _mockOwnedDeviceRepository.Setup(x => x.GetByHardwareId(id.ToString())).Returns((OwnedDevice)null);

        // Act
        var act = () => _notificationService.Notify(args);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
