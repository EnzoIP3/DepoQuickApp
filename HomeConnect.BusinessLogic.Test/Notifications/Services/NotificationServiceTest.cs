using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.Notifications.Repositories;
using BusinessLogic.Notifications.Services;
using Moq;

namespace HomeConnect.BusinessLogic.Test.Notifications.Services;

[TestClass]

public class NotificationServiceTest
{
    private Mock<INotificationRepository> _mockNotificationRepository = null!;
    private NotificationService _notificationService = null!;
    [TestInitialize]
    public void TestInitialize()
    {
        _mockNotificationRepository = new Mock<INotificationRepository>();
        _notificationService = new NotificationService(_mockNotificationRepository.Object);
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
}
