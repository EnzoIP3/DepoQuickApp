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
using HomeConnect.WebApi.Controllers.Lamps;
using HomeConnect.WebApi.Controllers.Lamps.Models;
using Microsoft.AspNetCore.Http;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class LampControllerTests
{
    private readonly string _modelNumber = "123";

    private Mock<IBusinessOwnerService> _businessOwnerServiceMock = null!;
    private Mock<IDeviceService> _deviceServiceMock = null!;
    private Mock<HttpContext> _httpContextMock = null!;
    private LampController _lampController = null!;
    private Mock<INotificationService> _notificationServiceMock = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _httpContextMock = new Mock<HttpContext>();
        _businessOwnerServiceMock = new Mock<IBusinessOwnerService>(MockBehavior.Strict);
        _deviceServiceMock = new Mock<IDeviceService>(MockBehavior.Strict);
        _notificationServiceMock = new Mock<INotificationService>(MockBehavior.Strict);
        _lampController =
            new LampController(_businessOwnerServiceMock.Object, _deviceServiceMock.Object,
                _notificationServiceMock.Object)
            { ControllerContext = { HttpContext = _httpContextMock.Object } };
    }

    #region CreateDevices

    [TestMethod]
    public void CreateLamp_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var user = new User();
        var lamp = new Device("name", _modelNumber, "description", "http://example.com/photo.png", [],
            DeviceType.Lamp.ToString(),
            new Business());
        var lampArgs = new CreateDeviceArgs
        {
            Owner = user,
            Description = lamp.Description,
            MainPhoto = lamp.MainPhoto,
            ModelNumber = lamp.ModelNumber,
            Name = lamp.Name,
            SecondaryPhotos = lamp.SecondaryPhotos,
            Type = lamp.Type.ToString()
        };
        var lampRequest = new CreateLampRequest
        {
            Description = lamp.Description,
            MainPhoto = lamp.MainPhoto,
            ModelNumber = lamp.ModelNumber,
            Name = lamp.Name,
            SecondaryPhotos = lamp.SecondaryPhotos
        };
        _businessOwnerServiceMock.Setup(x => x.CreateDevice(lampArgs)).Returns(lamp);
        var items = new Dictionary<object, object?> { { Item.UserLogged, user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);

        // Act
        CreateLampResponse response = _lampController.CreateLamp(lampRequest);

        // Assert
        _businessOwnerServiceMock.VerifyAll();
        response.Should().NotBeNull();
        response.Id.Should().Be(lamp.Id);
    }

    #endregion

    #region TurningLamps

    [TestMethod]
    public void TurnOn_WhenCalledWithValidHardwareId_ReturnsOkResponse()
    {
        // Arrange
        var hardwareId = Guid.NewGuid().ToString();
        DateTime date = DateTime.Now;
        var state = true;
        var args = new NotificationArgs { HardwareId = hardwareId, Date = date, Event = "Lamp was turned on" };
        _deviceServiceMock.Setup(x => x.TurnLamp(hardwareId, state));
        _notificationServiceMock.Setup(x => x.SendLampNotification(It.IsAny<NotificationArgs>(), state));

        // Act
        NotifyResponse result = _lampController.TurnOn(hardwareId);

        // Assert
        _deviceServiceMock.Verify(x => x.TurnLamp(hardwareId, state), Times.Once);
        _notificationServiceMock.Verify(
            x => x.SendLampNotification(
                It.Is<NotificationArgs>(x => x.HardwareId == hardwareId && x.Event == args.Event), state), Times.Once);
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
    }

    [TestMethod]
    public void TurnOff_WhenCalledWithValidHardwareId_ReturnsOkResponse()
    {
        // Arrange
        var hardwareId = Guid.NewGuid().ToString();
        DateTime date = DateTime.Now;
        var state = false;
        var args = new NotificationArgs { HardwareId = hardwareId, Date = date, Event = "Lamp was turned off" };
        _deviceServiceMock.Setup(x => x.TurnLamp(hardwareId, state));
        _notificationServiceMock.Setup(x => x.SendLampNotification(It.IsAny<NotificationArgs>(), state));

        // Act
        NotifyResponse result = _lampController.TurnOff(hardwareId);

        // Assert
        _deviceServiceMock.Verify(x => x.TurnLamp(hardwareId, state), Times.Once);
        _notificationServiceMock.Verify(
            x => x.SendLampNotification(
                It.Is<NotificationArgs>(x => x.HardwareId == hardwareId && x.Event == args.Event), state), Times.Once);
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
    }

    #endregion
}
