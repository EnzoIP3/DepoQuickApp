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
    private Mock<INotificationService> _notificationServiceMock = null!;
    private Mock<HttpContext> _httpContextMock = null!;
    private SensorController _sensorController = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _httpContextMock = new Mock<HttpContext>();
        _deviceServiceMock = new Mock<IDeviceService>();
        _businessOwnerServiceMock = new Mock<IBusinessOwnerService>();
        _notificationServiceMock = new Mock<INotificationService>();
        _sensorController = new SensorController(_deviceServiceMock.Object,
            _businessOwnerServiceMock.Object,
            _notificationServiceMock.Object)
        { ControllerContext = { HttpContext = _httpContextMock.Object } };
    }

    #region CreateDevices

    [TestMethod]
    public void CreateSensor_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var user = new User();
        var sensor = new Device("name", "123", "description", "http://example.com/photo.png", [],
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
            Type = sensor.Type.ToString(),
        };
        var sensorRequest = new CreateSensorRequest
        {
            Description = sensor.Description,
            MainPhoto = sensor.MainPhoto,
            ModelNumber = sensor.ModelNumber,
            Name = sensor.Name,
            SecondaryPhotos = sensor.SecondaryPhotos,
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
    public void Open_WhenHardwareIdIsValid_ReturnsNotifyResponse()
    {
        // Arrange
        var hardwareId = "hardwareId";
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "Sensor was opened" };
        _deviceServiceMock.Setup(x => x.UpdateSensorState(hardwareId, true, It.IsAny<NotificationArgs>()));
        _notificationServiceMock.Setup(x => x.SendSensorNotification(It.IsAny<NotificationArgs>(), false));

        // Act
        NotifyResponse result = _sensorController.Open(hardwareId);

        // Assert
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
        _deviceServiceMock.Verify(x => x.UpdateSensorState(hardwareId, true, It.Is((NotificationArgs a) =>
            a.HardwareId == hardwareId && a.Event == args.Event)));
        _notificationServiceMock.Verify(x => x.SendSensorNotification(It.Is((NotificationArgs a) =>
            a.HardwareId == hardwareId && a.Event == args.Event), true));
    }

    [TestMethod]
    public void Close_WhenHardwareIdIsValid_ReturnsNotifyResponse()
    {
        // Arrange
        var hardwareId = "hardwareId";
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "Sensor was closed" };
        _deviceServiceMock.Setup(x => x.UpdateSensorState(hardwareId, false, It.IsAny<NotificationArgs>()));
        _notificationServiceMock.Setup(x => x.SendSensorNotification(It.IsAny<NotificationArgs>(), false));

        // Act
        NotifyResponse result = _sensorController.Close(hardwareId);

        // Assert
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
        _deviceServiceMock.Verify(x => x.UpdateSensorState(hardwareId, false, It.Is((NotificationArgs a) =>
            a.HardwareId == hardwareId && a.Event == args.Event)));
        _notificationServiceMock.Verify(x => x.SendSensorNotification(It.Is((NotificationArgs a) =>
            a.HardwareId == hardwareId && a.Event == args.Event), false));
    }
    #endregion
}
