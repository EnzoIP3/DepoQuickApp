using BusinessLogic.HomeOwners.Services;
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
    public void AddOwnedDeviceToRoom_WhenCalledWithValidRequest_ReturnsExpectedResponse()
    {
        // Arrange
        var roomId = "123e4567-e89b-12d3-a456-426614174000";
        var deviceId = "123e4567-e89b-12d3-a456-426614174001";
        var hardwareId = Guid.NewGuid();

        _homeOwnerService.Setup(x => x.AddOwnedDeviceToRoom(roomId, deviceId)).Returns(hardwareId);

        var request = new AddOwnedDeviceToRoomRequest { DeviceId = deviceId };

        // Act
        var response = _controller.AddOwnedDeviceToRoom(roomId, request);

        // Assert
        _homeOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.DeviceId.Should().Be(hardwareId.ToString());
        response.RoomId.Should().Be(roomId);
    }
}
