using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Services;
using BusinessLogic.Notifications.Models;
using BusinessLogic.Notifications.Services;
using BusinessLogic.Users.Services;
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
    private Mock<IBusinessOwnerService> _businessOwnerService = null!;
    private Mock<IUserService> _userService = null!;
    private CameraController _cameraController = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _deviceServiceMock = new Mock<IDeviceService>();
        _notificationServiceMock = new Mock<INotificationService>();
        _businessOwnerService = new Mock<IBusinessOwnerService>();
        _userService = new Mock<IUserService>();
        _cameraController = new CameraController(_notificationServiceMock.Object, _deviceServiceMock.Object,
            _businessOwnerService.Object, _userService.Object);
    }

    [TestMethod]
    public void MovementDetected_WithHardwareId_ReturnsNotifyResponse()
    {
        // Arrange
        var hardwareId = "hardwareId";
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "movement-detected" };
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
        _userService.Setup(x => x.Exists(request.UserId)).Returns(true);

        // Act
        var result = _cameraController.PersonDetected(hardwareId, request);

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
            HardwareId = hardwareId,
            Date = DateTime.Now,
            Event = $"person detected with id: {request.UserId}"
        };
        _notificationServiceMock.Setup(x => x.Notify(args)).Throws<ArgumentException>();
        _userService.Setup(x => x.Exists(request.UserId)).Returns(false);

        // Act
        var act = () => _cameraController.PersonDetected(hardwareId, request);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("User detected by camera is not found");
    }

    [TestMethod]
    public void CreateCamera_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var camera = new Camera("Name", 123, "Description", "https://example.com/photo.png", [],
            new Business(), true, true,
            true,
            true);
        var cameraArgs = new CreateCameraArgs()
        {
            Name = "Name",
            BusinessRut = "306869575",
            Description = "Description",
            IsExterior = true,
            IsInterior = true,
            MainPhoto = "MainPhoto",
            ModelNumber = 123,
            MotionDetection = true,
            PersonDetection = true,
            SecondaryPhotos = []
        };
        var cameraRequest = new CreateCameraRequest
        {
            Name = cameraArgs.Name,
            BusinessRut = cameraArgs.BusinessRut,
            Description = cameraArgs.Description,
            IsExterior = cameraArgs.IsExterior,
            IsInterior = cameraArgs.IsInterior,
            MainPhoto = cameraArgs.MainPhoto,
            ModelNumber = cameraArgs.ModelNumber,
            MotionDetection = cameraArgs.MotionDetection,
            PersonDetection = cameraArgs.PersonDetection,
            SecondaryPhotos = cameraArgs.SecondaryPhotos
        };
        _businessOwnerService.Setup(x => x.CreateCamera(cameraArgs)).Returns(camera.Id);

        // Act
        var response = _cameraController.CreateCamera(cameraRequest);

        // Assert
        _businessOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.Id.Should().Be(camera.Id);
    }
}
