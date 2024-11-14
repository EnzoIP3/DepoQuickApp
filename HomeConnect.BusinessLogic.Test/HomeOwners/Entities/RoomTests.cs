using System;
using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Entities;
using FluentAssertions;

[TestClass]
public class RoomTests
{
    [TestMethod]
    public void Room_WhenInitializedWithValidParameters_ShouldSetProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Living Room";
        var home = new Home();
        var devices = new List<OwnedDevice>
        {
            new OwnedDevice()
        };

        // Act
        var room = new Room(id, name, home, devices);

        // Assert
        room.Id.Should().Be(id);
        room.Name.Should().Be(name);
        room.Home.Should().Be(home);
        room.OwnedDevices.Should().BeEquivalentTo(devices);
    }

    [TestMethod]
    public void Room_WhenNameIsEmpty_ShouldThrowArgumentException()
    {
        // Act
        Action act = () => { var room = new Room { Id = Guid.NewGuid(), Name = string.Empty }; };

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Room name cannot be null or empty.");
    }

    [TestMethod]
    public void Room_WhenHomeIsNull_ShouldThrowArgumentException()
    {
        // Act
        Action act = () => { var room = new Room { Id = Guid.NewGuid(), Name = "Living Room", Home = null }; };

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Room must have a home assigned.");
    }

    [TestMethod]
    public void Room_ShouldAllowAddingOwnedDevices()
    {
        // Arrange
        var room = new Room { Id = Guid.NewGuid(), Name = "Living Room", Home = new Home() };
        var device = new OwnedDevice();

        // Act
        room.OwnedDevices.Add(device);

        // Assert
        room.OwnedDevices.Should().Contain(device);
    }
}
