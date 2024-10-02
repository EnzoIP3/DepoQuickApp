using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Entities;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test.Notifications.Entities;

[TestClass]
public class NotificationTest
{
    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_CreatesInstance()
    {
        // Arrange
        var ownedDevice = new OwnedDevice(new Home(new global::BusinessLogic.Users.Entities.User(), "Main St 123", 12.5, 12.5, 5),
            new global::BusinessLogic.Devices.Entities.Device("Sensor", 12345, "A sensor", "https://sensor.com/image.png", [], "Sensor", new Business()));

        // Act
        var act = () => new global::BusinessLogic.Notifications.Entities.Notification(Guid.NewGuid(), DateTime.Now, false, "Event", ownedDevice, new global::BusinessLogic.Users.Entities.User());

        // Assert
        act.Should().NotThrow();
    }
}
