using BusinessLogic;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public class OwnedDeviceTests
{
    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_CreatesInstance()
    {
        // Arrange
        var home = new Home(new User(), "Main St 123", 12.5, 12.5, 5);
        var device = new Device("Sensor", 12345, "A sensor", "https://sensor.com/image.png", [],
            "Sensor");

        // Act
        var act = () => new OwnedDevice(home, device);

        // Assert
        act.Should().NotThrow();
    }

    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_GeneratesHardwareId()
    {
        // Arrange
        var home = new Home(new User(), "Main St 123", 12.5, 12.5, 5);
        var device = new Device("Sensor", 12345, "A sensor", "https://sensor.com/image.png", [],
            "Sensor");

        // Act
        var ownedDevice = new OwnedDevice(home, device);

        // Assert
        ownedDevice.HardwareId.Should().NotBeEmpty();
    }
}
