using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test.Devices;

[TestClass]
public class OwnedDeviceTests
{
    #region Constructor

    #region Success

    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_CreatesInstance()
    {
        // Arrange
        var home = new Home(new User(), "Main St 123", 12.5, 12.5, 5);
        var device = new Device("Sensor", 12345, "A sensor", "https://sensor.com/image.png", [], "Sensor",
            new Business());

        // Act
        Func<OwnedDevice> act = () => new OwnedDevice(home, device);

        // Assert
        act.Should().NotThrow();
    }

    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_GeneratesHardwareId()
    {
        // Arrange
        var home = new Home(new User(), "Main St 123", 12.5, 12.5, 5);
        var device = new Device("Sensor", 12345, "A sensor", "https://sensor.com/image.png", [], "Sensor",
            new Business());

        // Act
        var ownedDevice = new OwnedDevice(home, device);

        // Assert
        ownedDevice.HardwareId.Should().NotBeEmpty();
    }

    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_SetsConnected()
    {
        // Arrange
        var home = new Home(new User(), "Main St 123", 12.5, 12.5, 5);
        var device = new Device("Sensor", 12345, "A sensor", "https://sensor.com/image.png", [], "Sensor",
            new Business());

        // Act
        var ownedDevice = new OwnedDevice(home, device);

        // Assert
        ownedDevice.Connected.Should().BeTrue();
    }

    #endregion

    #endregion
}
