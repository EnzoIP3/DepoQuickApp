using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Services;
using BusinessLogic.Users.Services;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Rooms;
using HomeConnect.WebApi.Controllers.Rooms.Models;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class RoomControllerTests
{
    private Mock<IHomeOwnerService> _homeOwnerService = null!;
    private RoomController _controller = null!;

    [TestInitialize]
    public void Initialize()
    {
        _homeOwnerService = new Mock<IHomeOwnerService>(MockBehavior.Strict);
        _controller = new RoomController(_homeOwnerService.Object);
    }

    [TestMethod]
    public void CreateRoom_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var homeId = "123e4567-e89b-12d3-a456-426614174000";
        var name = "Living Room";
        var room = new Room
        {
            Id = Guid.NewGuid(),
            Name = name
        };

        _homeOwnerService.Setup(x => x.CreateRoom(It.IsAny<string>(), name)).Returns(room);

        var args = new AddRoomArgs
        {
            HomeId = homeId,
            Name = name
        };

        // Act
        var response = _controller.CreateRoom(args);

        // Assert
        _homeOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.RoomId.Should().Be(room.Id.ToString());
    }
}
