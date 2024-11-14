using BusinessLogic.HomeOwners.Entities;
using System;
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
}
