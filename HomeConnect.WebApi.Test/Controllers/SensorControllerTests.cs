using BusinessLogic.Devices.Services;
using BusinessLogic.Notifications.Services;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Sensor;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class SensorControllerTests
{
    private Mock<INotificationService> _notificationServiceMock = null!;
    private Mock<IDeviceService> _deviceServiceMock = null!;
    private SensorController _sensorController = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _notificationServiceMock = new Mock<INotificationService>();
        _deviceServiceMock = new Mock<IDeviceService>();
        _sensorController = new SensorController(_notificationServiceMock.Object, _deviceServiceMock.Object);
    }

    [TestMethod]
    public void NotifyOpen_WithHardwareId_ReturnsNotifyResponse()
    {
        // Arrange
        var hardwareId = "hardwareId";
        var args = new NotificationArgs
        {
            HardwareId = hardwareId,
            Date = DateTime.Now,
            Event = "open"
        };
        _notificationServiceMock.Setup(x => x.Notify(args));

        // Act
        var result = _sensorController.NotifyOpen(hardwareId);

        // Assert
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
    }

    [TestMethod]
    public void NotifyClose_WithHardwareId_ReturnsNotifyResponse()
    {
        // Arrange
        var hardwareId = "hardwareId";
        var args = new NotificationArgs
        {
            HardwareId = hardwareId,
            Date = DateTime.Now,
            Event = "close"
        };
        _notificationServiceMock.Setup(x => x.Notify(args));

        // Act
        var result = _sensorController.NotifyOpen(hardwareId);

        // Assert
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
    }

    [TestMethod]
    public void Toggle_WithHardwareId_ReturnsConnectionResponse()
    {
        // Arrange
        var hardwareId = "hardwareId";
        _deviceServiceMock.Setup(x => x.Toogle(hardwareId)).Returns(true);

        // Act
        var result = _sensorController.Toggle(hardwareId);

        // Assert
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
        result.ConnectionState.Should().BeTrue();
    }
}
