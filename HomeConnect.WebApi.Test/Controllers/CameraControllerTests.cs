using BusinessLogic.Devices.Services;
using BusinessLogic.Notifications.Services;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Camera;
using HomeConnect.WebApi.Controllers.Camera.Models;
using HomeConnect.WebApi.Controllers.Sensor;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class CameraControllerTests
{
    private Mock<IDeviceService> _deviceServiceMock = null!;
    private Mock<INotificationService> _notificationServiceMock = null!;
    private CameraController _cameraController = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _deviceServiceMock = new Mock<IDeviceService>();
        _notificationServiceMock = new Mock<INotificationService>();
        _cameraController = new CameraController(_notificationServiceMock.Object, _deviceServiceMock.Object);
    }

    [TestMethod]
    public void MovementDetected_WithHardwareId_ReturnsNotifyResponse()
    {
        // Arrange
        var hardwareId = "hardwareId";
        var args = new NotificationArgs
        {
            HardwareId = hardwareId,
            Date = DateTime.Now,
            Event = "movement-detected"
        };
        _notificationServiceMock.Setup(x => x.Notify(args));

        // Act
        var result = _cameraController.MovementDetected(hardwareId);

        // Assert
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
    }

    [TestMethod]
    public void PersonDetected_WithHardwareIdAndRequest_ReturnsNotifyResponse()
    {
        // Arrange
        var hardwareId = "hardwareId";
        var request = new PersonDetectedRequest { UserId = "userId" };
        var args = new NotificationArgs
        {
            HardwareId = hardwareId,
            Date = DateTime.Now,
            Event = $"person detected with id: {request.UserId}"
        };
        _notificationServiceMock.Setup(x => x.Notify(args));

        // Act
        var result = _cameraController.PersonDetected(hardwareId, request);

        // Assert
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
    }
}
