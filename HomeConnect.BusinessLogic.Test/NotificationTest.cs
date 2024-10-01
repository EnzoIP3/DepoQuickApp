using BusinessLogic;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public class NotificationTest
{
[TestMethod]
    public void Constructor_WhenArgumentsAreValid_CreatesInstance()
    {
        // Arrange
        var ownedDevice = new OwnedDevice(new Home(new User(), "Main St 123", 12.5, 12.5, 5),
            new Device("Sensor", 12345, "A sensor", "https://sensor.com/image.png", [], "Sensor"));

        // Act
        var act = () => new Notification(Guid.NewGuid(), DateTime.Now, false, "Event", ownedDevice, new User());

        // Assert
        act.Should().NotThrow();
    }
}
