using System;
using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Entities;
using FluentAssertions;

[TestClass]
public class RoomTests
{
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
