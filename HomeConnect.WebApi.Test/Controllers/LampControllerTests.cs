using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Services;
using BusinessLogic.Notifications.Models;
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
    private Mock<IBusinessOwnerService> _businessOwnerServiceMock = null!;
    private Mock<IDeviceService> _deviceServiceMock = null!;
    private Mock<HttpContext> _httpContextMock = null!;
    private LampController _lampController = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _httpContextMock = new Mock<HttpContext>();
        _businessOwnerServiceMock = new Mock<IBusinessOwnerService>();
        _deviceServiceMock = new Mock<IDeviceService>();
        _lampController =
            new LampController(_businessOwnerServiceMock.Object, _deviceServiceMock.Object)
            { ControllerContext = { HttpContext = _httpContextMock.Object } };
    }

    #region CreateDevices

    [TestMethod]
    public void CreateLamp_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var user = new User();
        var lamp = new Device("name", 123, "description", "http://example.com/photo.png", [],
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
        var date = DateTime.Now;
        var state = true;
        var args = new NotificationArgs { HardwareId = hardwareId, Date = date, Event = "lamp-turned-on" };
        _deviceServiceMock.Setup(x => x.TurnLamp(hardwareId, state, args));

        // Act
        NotifyResponse result = _lampController.TurnOn(hardwareId);

        // Assert
        _deviceServiceMock.Verify(x => x.TurnLamp(hardwareId, state, It.Is<NotificationArgs>(y =>
            y.HardwareId == hardwareId &&
            y.Date.Year == date.Year &&
            y.Date.Month == date.Month &&
            y.Date.Day == date.Day &&
            y.Date.Hour == date.Hour &&
            y.Date.Minute == date.Minute &&
            y.Event == args.Event)), Times.Once);
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
    }

    [TestMethod]
    public void TurnOff_WhenCalledWithValidHardwareId_ReturnsOkResponse()
    {
        // Arrange
        var hardwareId = Guid.NewGuid().ToString();
        var date = DateTime.Now;
        var state = false;
        var args = new NotificationArgs { HardwareId = hardwareId, Date = date, Event = "lamp-turned-off" };
        _deviceServiceMock.Setup(x => x.TurnLamp(hardwareId, state, args));

        // Act
        NotifyResponse result = _lampController.TurnOff(hardwareId);

        // Assert
        _deviceServiceMock.Verify(x => x.TurnLamp(hardwareId, state, It.Is<NotificationArgs>(y =>
            y.HardwareId == hardwareId &&
            y.Date.Year == date.Year &&
            y.Date.Month == date.Month &&
            y.Date.Day == date.Day &&
            y.Date.Hour == date.Hour &&
            y.Date.Minute == date.Minute &&
            y.Event == args.Event)), Times.Once);
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
    }

    #endregion
}
