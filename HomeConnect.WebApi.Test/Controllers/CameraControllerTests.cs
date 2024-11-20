using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Services;
using BusinessLogic.Notifications.Models;
using BusinessLogic.Notifications.Services;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Cameras;
using HomeConnect.WebApi.Controllers.Cameras.Models;
using HomeConnect.WebApi.Controllers.Devices.Models;
using Microsoft.AspNetCore.Http;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class CameraControllerTests
{
    private Mock<IBusinessOwnerService> _businessOwnerService = null!;
    private CameraController _cameraController = null!;
    private Mock<IDeviceService> _deviceService = null!;
    private Mock<HttpContext> _httpContext = null!;
    private Mock<INotificationService> _notificationService = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _httpContext = new Mock<HttpContext>();
        _deviceService = new Mock<IDeviceService>();
        _notificationService = new Mock<INotificationService>();
        _businessOwnerService = new Mock<IBusinessOwnerService>();
        _cameraController = new CameraController(_notificationService.Object, _deviceService.Object,
            _businessOwnerService.Object) { ControllerContext = { HttpContext = _httpContext.Object } };
    }

    #region CreateCamera

    [TestMethod]
    public void CreateCamera_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var user = new User();
        var camera = new Camera("Name", "123", "Description", "https://example.com/photo.png", [],
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
            ModelNumber = "123",
            MotionDetection = true,
            PersonDetection = true,
            SecondaryPhotos = [],
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
            SecondaryPhotos = cameraArgs.SecondaryPhotos,
        };
        _businessOwnerService.Setup(x => x.CreateCamera(cameraArgs)).Returns(camera);
        var items = new Dictionary<object, object?> { { Item.UserLogged, user } };
        _httpContext.Setup(h => h.Items).Returns(items);

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
        var args = new NotificationArgs { HardwareId = hardwareId, Date = DateTime.Now, Event = "Movement detected" };
        _deviceService.Setup(x => x.IsConnected(hardwareId)).Returns(true);
        _notificationService.Setup(x => x.Notify(args, _deviceService.Object));

        // Act
        NotifyResponse result = _cameraController.MovementDetected(hardwareId);

        // Assert
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
    }

    #endregion

    #region PersonDetected

    [TestMethod]
    public void PersonDetected_WithHardwareIdAndRequest_ReturnsNotifyResponse()
    {
        // Arrange
        var hardwareId = "hardwareId";
        var request = new PersonDetectedRequest { UserEmail = "email@example.com" };
        var args = new NotificationArgs
        {
            HardwareId = hardwareId, Date = DateTime.Now, Event = $"Person detected with email: {request.UserEmail}"
        };
        _deviceService.Setup(x => x.IsConnected(hardwareId)).Returns(true);
        _notificationService.Setup(x => x.Notify(args, _deviceService.Object));

        // Act
        NotifyResponse result = _cameraController.PersonDetected(hardwareId, request);

        // Assert
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
    }

    #endregion

    #region GetCamera

    [TestMethod]
    public void GetCamera_WithCameraId_ReturnsGetCameraResponse()
    {
        // Arrange
        var cameraId = "cameraId";
        var camera = new Camera("Name", "123", "Description", "https://example.com/photo.png", [],
            new Business(), true, true,
            true,
            true);
        _deviceService.Setup(x => x.GetCameraById(cameraId)).Returns(camera);

        // Act
        GetCameraResponse result = _cameraController.GetCamera(cameraId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(camera.Id.ToString());
        result.Name.Should().Be(camera.Name);
        result.Description.Should().Be(camera.Description);
        result.Exterior.Should().Be(camera.IsExterior);
        result.Interior.Should().Be(camera.IsInterior);
        result.MainPhoto.Should().Be(camera.MainPhoto);
        result.ModelNumber.Should().Be(camera.ModelNumber);
        result.MotionDetection.Should().Be(camera.MotionDetection);
        result.PersonDetection.Should().Be(camera.PersonDetection);
        result.SecondaryPhotos.Should().BeEquivalentTo(camera.SecondaryPhotos);
    }

    #endregion
}
