using BusinessLogic;
using Moq;

namespace HomeConnect.BusinessLogic.Test;

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
        var user = new User("name", "surname", "email@email.com", "Password#100", new Role());
        var device = new Device("Device", 12345, "Device description", "https://example.com/image.png",
            new List<string>(), "Sensor");
        var home = new Home(user, "Adress 3420", 100, 100, 5);
        var ownedDevice = new OwnedDevice(home, device);
        var @event = "Test Event";

        // Act
        _notificationService.CreateNotification(ownedDevice, @event, user);

        // Assert
        _mockNotificationRepository.Verify(x => x.Add(It.Is<Notification>(n =>
            n.Event == @event &&
            n.User == user &&
            n.OwnedDevice == ownedDevice)), Times.Once);
    }
}
