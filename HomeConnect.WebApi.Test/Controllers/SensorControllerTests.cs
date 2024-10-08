using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Services;
using BusinessLogic.Notifications.Models;
using BusinessLogic.Notifications.Services;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Devices.Models;
using HomeConnect.WebApi.Controllers.Sensors;
using HomeConnect.WebApi.Controllers.Sensors.Models;
using Microsoft.AspNetCore.Http;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class SensorControllerTests
{
    private Mock<IBusinessOwnerService> _businessOwnerServiceMock = null!;
    private Mock<IDeviceService> _deviceServiceMock = null!;
    private Mock<HttpContext> _httpContextMock = null!;
    private Mock<INotificationService> _notificationServiceMock = null!;
    private SensorController _sensorController = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _httpContextMock = new Mock<HttpContext>();
        _notificationServiceMock = new Mock<INotificationService>();
        _deviceServiceMock = new Mock<IDeviceService>();
        _businessOwnerServiceMock = new Mock<IBusinessOwnerService>();
        _sensorController = new SensorController(_notificationServiceMock.Object, _deviceServiceMock.Object,
            _businessOwnerServiceMock.Object) { ControllerContext = { HttpContext = _httpContextMock.Object } };
    }

    #region CreateDevices

    [TestMethod]
    public void CreateSensor_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var user = new User();
        var sensor = new Device("name", 123, "description", "http://example.com/photo.png", [],
            DeviceType.Sensor.ToString(),
            new Business());
        var sensorArgs = new CreateDeviceArgs
        {
            Owner = user,
            Description = sensor.Description,
            MainPhoto = sensor.MainPhoto,
            ModelNumber = sensor.ModelNumber,
            Name = sensor.Name,
            SecondaryPhotos = sensor.SecondaryPhotos,
            Type = sensor.Type.ToString()
        };
        var sensorRequest = new CreateSensorRequest
        {
            Description = sensor.Description,
            MainPhoto = sensor.MainPhoto,
            ModelNumber = sensor.ModelNumber,
            Name = sensor.Name,
            SecondaryPhotos = sensor.SecondaryPhotos
        };
        _businessOwnerServiceMock.Setup(x => x.CreateDevice(sensorArgs)).Returns(sensor);
        var items = new Dictionary<object, object?> { { Item.UserLogged, user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);

        // Act
        CreateSensorResponse response = _sensorController.CreateSensor(sensorRequest);

        // Assert
        _businessOwnerServiceMock.VerifyAll();
        response.Should().NotBeNull();
        response.Id.Should().Be(sensor.Id);
    }

    #endregion

    #region Notify

    [TestMethod]
    public void NotifyOpen_WhenHardwareIdIsValid_ReturnsNotifyResponse()
    {
        // Arrange
        var hardwareId = "hardwareId";
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "open" };
        _deviceServiceMock.Setup(x => x.IsConnected(hardwareId)).Returns(true);
        _notificationServiceMock.Setup(x => x.Notify(args));

        // Act
        NotifyResponse result = _sensorController.NotifyOpen(hardwareId);

        // Assert
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
    }

    [TestMethod]
    public void NotifyClose_WhenHardwareIdIsValid_ReturnsNotifyResponse()
    {
        // Arrange
        var hardwareId = "hardwareId";
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "close" };
        _deviceServiceMock.Setup(x => x.IsConnected(hardwareId)).Returns(true);
        _notificationServiceMock.Setup(x => x.Notify(args));

        // Act
        NotifyResponse result = _sensorController.NotifyClose(hardwareId);

        // Assert
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
    }

    [TestMethod]
    public void NotifyOpen_WhenDeviceIsDisconnected_ThrowsException()
    {
        // Arrange
        var hardwareId = "hardwareId";

        _deviceServiceMock.Setup(x => x.IsConnected(hardwareId)).Returns(false);

        // Act
        Func<NotifyResponse> act = () => _sensorController.NotifyOpen(hardwareId);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Device is not connected");
        _deviceServiceMock.VerifyAll();
    }

    [TestMethod]
    public void NotifyClose_WhenDeviceIsDisconnected_ThrowsException()
    {
        // Arrange
        var hardwareId = "hardwareId";
        _deviceServiceMock.Setup(x => x.IsConnected(hardwareId)).Returns(false);

        // Act
        Func<NotifyResponse> act = () => _sensorController.NotifyClose(hardwareId);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Device is not connected");
        _deviceServiceMock.VerifyAll();
    }

    #endregion
}
