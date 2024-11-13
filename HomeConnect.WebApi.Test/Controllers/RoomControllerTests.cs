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

    [TestMethod]
    public void AddOwnedDeviceToRoom_WhenCalledWithValidRequest_ReturnsExpectedResponse()
    {
        // Arrange
        var roomId = "123e4567-e89b-12d3-a456-426614174000";
        var deviceId = "123e4567-e89b-12d3-a456-426614174001";
        var hardwareId = Guid.NewGuid();

        _homeOwnerService.Setup(x => x.AddOwnedDeviceToRoom(roomId, deviceId)).Returns(hardwareId);

        var request = new AddOwnedDeviceToRoomRequest
        {
            DeviceId = deviceId
        };

        // Act
        var response = _controller.AddOwnedDeviceToRoom(roomId, request);

        // Assert
        _homeOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.DeviceId.Should().Be(hardwareId.ToString());
        response.RoomId.Should().Be(roomId);
    }

    [TestMethod]
    public void MoveDevice_WhenCalledWithValidRequest_ReturnsExpectedResponse()
    {
        // Arrange
        var sourceRoomId = "123e4567-e89b-12d3-a456-426614174000";
        var targetRoomId = "123e4567-e89b-12d3-a456-426614174001";
        var deviceId = "123e4567-e89b-12d3-a456-426614174002";

        _homeOwnerService.Setup(x => x.MoveDevice(sourceRoomId, targetRoomId, deviceId)).Verifiable();

        var request = new MoveDeviceRequest
        {
            SourceRoomId = sourceRoomId,
            TargetRoomId = targetRoomId
        };

        // Act
        var response = _controller.MoveDevice(deviceId, request);

        // Assert
        _homeOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.SourceRoomId.Should().Be(sourceRoomId);
        response.TargetRoomId.Should().Be(targetRoomId);
        response.DeviceId.Should().Be(deviceId);
    }
}
