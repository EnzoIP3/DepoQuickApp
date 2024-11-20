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
using HomeConnect.WebApi.Controllers.MotionSensors;
using HomeConnect.WebApi.Controllers.MotionSensors.Models;
using Microsoft.AspNetCore.Http;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class MotionSensorControllerTests
{
    private readonly string _modelNumber = "123";

    private Mock<IBusinessOwnerService> _businessOwnerServiceMock = null!;
    private Mock<IDeviceService> _deviceServiceMock = null!;
    private Mock<HttpContext> _httpContextMock = null!;
    private Mock<INotificationService> _notificationServiceMock = null!;
    private MotionSensorController _motionSensorController = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _httpContextMock = new Mock<HttpContext>();
        _notificationServiceMock = new Mock<INotificationService>();
        _deviceServiceMock = new Mock<IDeviceService>();
        _businessOwnerServiceMock = new Mock<IBusinessOwnerService>();
        _motionSensorController = new MotionSensorController(_notificationServiceMock.Object, _deviceServiceMock.Object, _businessOwnerServiceMock.Object)
        { ControllerContext = { HttpContext = _httpContextMock.Object } };
    }

    #region CreateDevices

    [TestMethod]
    public void CreateMotionSensor_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var user = new User();
        var motionSensor = new Device("name", _modelNumber, "description", "http://example.com/photo.png", [],
            DeviceType.MotionSensor.ToString(),
            new Business());
        var motionSensorArgs = new CreateDeviceArgs
        {
            Owner = user,
            Description = motionSensor.Description,
            MainPhoto = motionSensor.MainPhoto,
            ModelNumber = motionSensor.ModelNumber,
            Name = motionSensor.Name,
            SecondaryPhotos = motionSensor.SecondaryPhotos,
            Type = motionSensor.Type.ToString()
        };
        var motionSensorRequest = new CreateMotionSensorRequest
        {
            Description = motionSensor.Description,
            MainPhoto = motionSensor.MainPhoto,
            ModelNumber = motionSensor.ModelNumber,
            Name = motionSensor.Name,
            SecondaryPhotos = motionSensor.SecondaryPhotos
        };
        _businessOwnerServiceMock.Setup(x => x.CreateDevice(motionSensorArgs)).Returns(motionSensor);
        var items = new Dictionary<object, object?> { { Item.UserLogged, user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);

        // Act
        CreateMotionSensorResponse response = _motionSensorController.CreateMotionSensor(motionSensorRequest);

        // Assert
        _businessOwnerServiceMock.VerifyAll();
        response.Should().NotBeNull();
        response.Id.Should().Be(motionSensor.Id);
    }
    #endregion

    #region MovementDetected

    [TestMethod]
    public void MovementDetected_WithHardwareId_ReturnsNotifyResponse()
    {
        // Arrange
        var hardwareId = "hardwareId";
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "Movement detected" };
        _deviceServiceMock.Setup(x => x.IsConnected(hardwareId)).Returns(true);
        _notificationServiceMock.Setup(x => x.Notify(args, _deviceServiceMock.Object));

        // Act
        NotifyResponse result = _motionSensorController.MovementDetected(hardwareId);

        // Assert
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
    }
    #endregion
}
