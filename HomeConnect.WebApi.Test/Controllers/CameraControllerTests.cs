using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Services;
using BusinessLogic.Notifications.Models;
using BusinessLogic.Notifications.Services;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Services;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Camera;
using HomeConnect.WebApi.Controllers.Camera.Models;
using HomeConnect.WebApi.Controllers.Device.Models;
using Microsoft.AspNetCore.Http;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class CameraControllerTests
{
    private Mock<IBusinessOwnerService> _businessOwnerService = null!;
    private CameraController _cameraController = null!;
    private Mock<IDeviceService> _deviceServiceMock = null!;
    private Mock<HttpContext> _httpContextMock = null!;
    private Mock<INotificationService> _notificationServiceMock = null!;
    private Mock<IUserService> _userService = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _httpContextMock = new Mock<HttpContext>();
        _deviceServiceMock = new Mock<IDeviceService>();
        _notificationServiceMock = new Mock<INotificationService>();
        _businessOwnerService = new Mock<IBusinessOwnerService>();
        _userService = new Mock<IUserService>();
        _cameraController = new CameraController(_notificationServiceMock.Object, _deviceServiceMock.Object,
            _businessOwnerService.Object, _userService.Object)
        {
            ControllerContext = { HttpContext = _httpContextMock.Object }
        };
    }

    #region CreateCamera

    [TestMethod]
    public void CreateCamera_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var user = new User();
        var camera = new Camera("Name", 123, "Description", "https://example.com/photo.png", [],
            new Business(), true, true,
            true,
            true);
        var cameraArgs = new CreateCameraArgs
        {
            Name = "Name",
            Owner = user,
            Description = "Description",
            Exterior = true,
            Interior = true,
            MainPhoto = "MainPhoto",
            ModelNumber = 123,
            MotionDetection = true,
            PersonDetection = true,
            SecondaryPhotos = []
        };
        var cameraRequest = new CreateCameraRequest
        {
            Name = cameraArgs.Name,
            Description = cameraArgs.Description,
            Exterior = cameraArgs.Exterior,
            Interior = cameraArgs.Interior,
            MainPhoto = cameraArgs.MainPhoto,
            ModelNumber = cameraArgs.ModelNumber,
            MotionDetection = cameraArgs.MotionDetection,
            PersonDetection = cameraArgs.PersonDetection,
            SecondaryPhotos = cameraArgs.SecondaryPhotos
        };
        _businessOwnerService.Setup(x => x.CreateCamera(cameraArgs)).Returns(camera);
        var items = new Dictionary<object, object?> { { Item.UserLogged, user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);

        // Act
        CreateCameraResponse response = _cameraController.CreateCamera(cameraRequest);

        // Assert
        _businessOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.Id.Should().Be(camera.Id);
    }

    #endregion

    #region MovementDetected

    [TestMethod]
    public void MovementDetected_WithHardwareId_ReturnsNotifyResponse()
    {
        // Arrange
        var hardwareId = "hardwareId";
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "movement-detected" };
        _deviceServiceMock.Setup(x => x.IsConnected(hardwareId)).Returns(true);
        _notificationServiceMock.Setup(x => x.Notify(args));

        // Act
        NotifyResponse result = _cameraController.MovementDetected(hardwareId);

        // Assert
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
    }

    [TestMethod]
    public void MovementDetected_WhenCameraIsDisconnected_ThrowsArgumentException()
    {
        // Arrange
        var hardwareId = "hardwareId";
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "movement-detected" };
        _deviceServiceMock.Setup(x => x.IsConnected(hardwareId)).Returns(false);

        // Act
        Func<NotifyResponse> act = () => _cameraController.MovementDetected(hardwareId);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Device is not connected");
        _deviceServiceMock.VerifyAll();
    }

    #endregion

    #region PersonDetected

    [TestMethod]
    public void PersonDetected_WithHardwareIdAndRequest_ReturnsNotifyResponse()
    {
        // Arrange
        var hardwareId = "hardwareId";
        var request = new PersonDetectedRequest { UserId = "userId" };
        var args = new NotificationArgs
        {
            HardwareId = hardwareId, Date = DateTime.Now, Event = $"person detected with id: {request.UserId}"
        };
        _deviceServiceMock.Setup(x => x.IsConnected(hardwareId)).Returns(true);
        _notificationServiceMock.Setup(x => x.Notify(args));
        _userService.Setup(x => x.Exists(request.UserId)).Returns(true);

        // Act
        NotifyResponse result = _cameraController.PersonDetected(hardwareId, request);

        // Assert
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
    }

    [TestMethod]
    public void PersonDetected_WhenDetectedPersonIsNotRegistered_ThrowsArgumentException()
    {
        // Arrange
        var hardwareId = "hardwareId";
        var request = new PersonDetectedRequest { UserId = "userId" };
        var args = new NotificationArgs
        {
            HardwareId = hardwareId, Date = DateTime.Now, Event = $"person detected with id: {request.UserId}"
        };
        _deviceServiceMock.Setup(x => x.IsConnected(hardwareId)).Returns(true);
        _notificationServiceMock.Setup(x => x.Notify(args)).Throws<ArgumentException>();
        _userService.Setup(x => x.Exists(request.UserId)).Returns(false);

        // Act
        Func<NotifyResponse> act = () => _cameraController.PersonDetected(hardwareId, request);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("User detected by camera is not found");
    }

    [TestMethod]
    public void PersonDetected_WhenCameraIsDisconnected_ThrowsArgumentException()
    {
        // Arrange
        var hardwareId = "hardwareId";
        var request = new PersonDetectedRequest { UserId = "userId" };
        var args = new NotificationArgs
        {
            HardwareId = hardwareId, Date = DateTime.Now, Event = $"person detected with id: {request.UserId}"
        };
        _deviceServiceMock.Setup(x => x.IsConnected(hardwareId)).Returns(false);

        // Act
        Func<NotifyResponse> act = () => _cameraController.PersonDetected(hardwareId, request);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Device is not connected");
        _deviceServiceMock.VerifyAll();
    }

    #endregion
}
