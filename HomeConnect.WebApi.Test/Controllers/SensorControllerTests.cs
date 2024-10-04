using BusinessLogic.Notifications.Services;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Sensor;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class SensorControllerTests
{
    private Mock<INotificationService> _notificationServiceMock = null!;
    private SensorController _sensorController = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _notificationServiceMock = new Mock<INotificationService>();
        _sensorController = new SensorController(_notificationServiceMock.Object);
    }

    [TestMethod]
    public void Notify_WithHardwareId_ReturnsNotifyResponse()
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
        var result = _sensorController.Notify(hardwareId);

        // Assert
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
    }
}
