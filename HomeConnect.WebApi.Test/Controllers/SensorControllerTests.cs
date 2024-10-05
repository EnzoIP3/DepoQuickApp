using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Services;
using BusinessLogic.Notifications.Models;
using BusinessLogic.Notifications.Services;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Sensor;
using HomeConnect.WebApi.Controllers.Sensor.Models;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class SensorControllerTests
{
    private Mock<INotificationService> _notificationServiceMock = null!;
    private Mock<IDeviceService> _deviceServiceMock = null!;
    private Mock<IBusinessOwnerService> _businessOwnerServiceMock = null!;
    private SensorController _sensorController = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _notificationServiceMock = new Mock<INotificationService>();
        _deviceServiceMock = new Mock<IDeviceService>();
        _businessOwnerServiceMock = new Mock<IBusinessOwnerService>();
        _sensorController = new SensorController(_notificationServiceMock.Object, _deviceServiceMock.Object,
            _businessOwnerServiceMock.Object);
    }

    [TestMethod]
    public void NotifyOpen_WithHardwareId_ReturnsNotifyResponse()
    {
        // Arrange
        var hardwareId = "hardwareId";
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "open" };
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
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "close" };
        _notificationServiceMock.Setup(x => x.Notify(args));

        // Act
        var result = _sensorController.NotifyOpen(hardwareId);

        // Assert
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
    }

    #region CreateDevices

    [TestMethod]
    public void CreateSensor_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var sensor = new Device("name", 123, "description", "http://example.com/photo.png", [], DeviceType.Sensor.ToString(),
            new Business());
        var sensorArgs = new CreateDeviceArgs()
        {
            BusinessRut = sensor.Business.Rut,
            Description = sensor.Description,
            MainPhoto = sensor.MainPhoto,
            ModelNumber = sensor.ModelNumber,
            Name = sensor.Name,
            SecondaryPhotos = sensor.SecondaryPhotos,
            Type = sensor.Type.ToString()
        };
        var sensorRequest = new CreateSensorRequest()
        {
            BusinessRut = sensor.Business.Rut,
            Description = sensor.Description,
            MainPhoto = sensor.MainPhoto,
            ModelNumber = sensor.ModelNumber,
            Name = sensor.Name,
            SecondaryPhotos = sensor.SecondaryPhotos
        };
        _businessOwnerServiceMock.Setup(x => x.CreateDevice(sensorArgs)).Returns(sensor.Id);

        // Act
        var response = _sensorController.CreateSensor(sensorRequest);

        // Assert
        _businessOwnerServiceMock.VerifyAll();
        response.Should().NotBeNull();
        response.Id.Should().Be(sensor.Id);
    }

    #endregion
}
