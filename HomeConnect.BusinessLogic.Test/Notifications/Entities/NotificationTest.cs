using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.Notifications.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test.Notifications.Entities;

[TestClass]
public class NotificationTest
{
    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_CreatesInstance()
    {
        // Arrange
        var ownedDevice = new OwnedDevice(new Home(new User(), "Main St 123", 12.5, 12.5, 5),
            new Device("Sensor", "12345", "A sensor", "https://sensor.com/image.png", [], "Sensor", new Business()));

        // Act
        Func<Notification> act = () =>
            new Notification(Guid.NewGuid(), DateTime.Now, false, "Event", ownedDevice, new User());

        // Assert
        act.Should().NotThrow();
    }

    [TestMethod]
    public void Clone_WhenCalled_ReturnsNewInstance()
    {
        // Arrange
        var ownedDevice = new OwnedDevice(new Home(new User(), "Main St 123", 12.5, 12.5, 5),
            new Device("Sensor", "12345", "A sensor", "https://sensor.com/image.png", [], "Sensor", new Business()));
        var notification = new Notification(Guid.NewGuid(), DateTime.Now, false, "Event", ownedDevice, new User());

        // Act
        var clone = notification.Clone();

        // Assert
        clone.Should().NotBeSameAs(notification);
        clone.Should().BeEquivalentTo(notification);
    }
}
