using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Entities;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test.HomeOwners.Entities;

[TestClass]
public class RoomTests
{
    #region Constructor

    #region Success

    [TestMethod]
    public void Constructor_WhenCalledWithValidParameters_InitializesRoom()
    {
        // Arrange
        var name = "Living Room";
        var home = new Home();

        // Act
        var room = new Room(name, home);

        // Assert
        room.Name.Should().Be(name);
        room.Home.Should().Be(home);
    }

    #endregion

    #region Error

    [TestMethod]
    public void Constructor_WhenNameIsEmpty_ThrowsArgumentException()
    {
        // Act
        var act = () => new Room { Id = Guid.NewGuid(), Name = string.Empty };

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Room name cannot be null or empty.");
    }

    [TestMethod]
    public void Constructor_WhenHomeIsNull_ThrowsArgumentException()
    {
        // Act
        var act = () => new Room { Id = Guid.NewGuid(), Name = "Living Room", Home = null };

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Room must have a home assigned.");
    }

    #endregion

    #endregion

    #region AddOwnedDevice

    #region Success

    [TestMethod]
    public void AddOwnedDevice_WhenCalled_AddsDeviceToOwnedDevices()
    {
        // Arrange
        var room = new Room { Id = Guid.NewGuid(), Name = "Living Room", Home = new Home() };
        var device = new OwnedDevice() { Home = room.Home };

        // Act
        room.AddOwnedDevice(device);

        // Assert
        room.OwnedDevices.Should().Contain(device);
    }

    #endregion

    #region Error

    [TestMethod]
    public void AddOwnedDevice_WhenDeviceDoesNotBelongToTheSameHome_ThrowsArgumentException()
    {
        // Arrange
        var room = new Room { Id = Guid.NewGuid(), Name = "Living Room", Home = new Home() };
        var device = new OwnedDevice { Home = new Home() };

        // Act
        var act = () => room.AddOwnedDevice(device);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Device must belong to the same home as the room.");
    }

    [TestMethod]
    public void AddOwnedDevice_WhenDeviceAlreadyBelongsToTheRoom_ThrowsArgumentException()
    {
        // Arrange
        var room = new Room { Id = Guid.NewGuid(), Name = "Living Room", Home = new Home() };
        var device = new OwnedDevice { Home = room.Home };

        // Act
        room.AddOwnedDevice(device);
        var act = () => room.AddOwnedDevice(device);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Device already belongs to the room.");
    }

    #endregion

    #endregion

    #region RemoveOwnedDevice

    #region Success

    [TestMethod]
    public void RemoveOwnedDevice_WhenCalled_RemovesDeviceFromOwnedDevices()
    {
        // Arrange
        var room = new Room { Id = Guid.NewGuid(), Name = "Living Room", Home = new Home() };
        var device = new OwnedDevice { Home = room.Home };
        room.AddOwnedDevice(device);

        // Act
        room.RemoveOwnedDevice(device);

        // Assert
        room.OwnedDevices.Should().NotContain(device);
    }

    #endregion

    #region Error

    [TestMethod]
    public void RemoveOwnedDevice_WhenDeviceDoesNotBelongToTheRoom_ThrowsArgumentException()
    {
        // Arrange
        var room = new Room { Id = Guid.NewGuid(), Name = "Living Room", Home = new Home() };
        var device = new OwnedDevice { Home = room.Home };

        // Act
        var act = () => room.RemoveOwnedDevice(device);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Device does not belong to the room.");
    }

    #endregion

    #endregion

    #region GetOwnedDevice

    #region Success

    [TestMethod]
    public void GetOwnedDevice_WhenDeviceBelongsToTheRoom_ReturnsDevice()
    {
        // Arrange
        var room = new Room { Id = Guid.NewGuid(), Name = "Living Room", Home = new Home() };
        var device = new OwnedDevice { Home = room.Home };
        room.AddOwnedDevice(device);

        // Act
        var result = room.GetOwnedDevice(device.HardwareId);

        // Assert
        result.Should().Be(device);
    }

    #endregion

    #region Error

    [TestMethod]
    public void GetOwnedDevice_WhenDeviceDoesNotBelongToTheRoom_ThrowsArgumentException()
    {
        // Arrange
        var room = new Room { Id = Guid.NewGuid(), Name = "Living Room", Home = new Home() };
        var device = new OwnedDevice { Home = new Home() };

        // Act
        var act = () => room.GetOwnedDevice(device.HardwareId);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Device does not belong to the room.");
    }

    #endregion

    #endregion
}
