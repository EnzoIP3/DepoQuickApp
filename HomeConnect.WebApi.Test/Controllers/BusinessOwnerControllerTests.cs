using BusinessLogic.Admins.Services;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Models;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.BusinessOwner;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class BusinessOwnerControllerTests
{
    private Mock<IAdminService> _adminService = null!;
    private Mock<IBusinessOwnerService> _businessOwnerService = null!;
    private BusinessOwnerController _controller = null!;
    private CreateBusinessOwnerRequest _businessOwnerRequest;
    private CreateUserArgs _userModel;
    private Guid _guid;
    private CreateBusinessRequest _businessRequest;
    private Business _business = null!;
    private CreateDeviceRequest _deviceRequest;
    private Device _device = null!;
    private CreateCameraRequest _cameraRequest;
    private Camera _camera = null!;

    [TestInitialize]
    public void Initialize()
    {
        _adminService = new Mock<IAdminService>();
        _businessOwnerService = new Mock<IBusinessOwnerService>();
        _controller = new BusinessOwnerController(_adminService.Object, _businessOwnerService.Object);

        _businessOwnerRequest = new CreateBusinessOwnerRequest
        {
            Name = "John",
            Surname = "Doe",
            Email = "email@email.com",
            Password = "password"
        };
        _userModel = new CreateUserArgs
        {
            Name = _businessOwnerRequest.Name,
            Surname = _businessOwnerRequest.Surname,
            Email = _businessOwnerRequest.Email,
            Password = _businessOwnerRequest.Password
        };
        _guid = Guid.NewGuid();
        _businessRequest = new CreateBusinessRequest
        {
            Name = "OSE",
            Rut = "306869575",
            Owner = new User { Id = Guid.NewGuid() }
        };
        _business = new Business
        {
            Name = _businessRequest.Name,
            Rut = _businessRequest.Rut,
            Owner = _businessRequest.Owner
        };
        _deviceRequest = new CreateDeviceRequest
        {
            Name = "Device1",
            ModelNumber = 123,
            Description = "Test device",
            MainPhoto = "https://www.example.com/photo1.jpg",
            SecondaryPhotos = new List<string> {},
            Type = "Camera",
            Business = new Business()
        };
        _device = new Device
        {
            Name = _deviceRequest.Name,
            ModelNumber = _deviceRequest.ModelNumber,
            Description = _deviceRequest.Description,
            MainPhoto = _deviceRequest.MainPhoto,
            SecondaryPhotos = _deviceRequest.SecondaryPhotos,
            Type = _deviceRequest.Type,
            Business = _deviceRequest.Business
        };
        _cameraRequest = new CreateCameraRequest
        {
            Name = "Camera1",
            ModelNumber = 123,
            Description = "Test camera",
            MainPhoto = "https://www.example.com/photo1.jpg",
            SecondaryPhotos = new List<string> {},
            MotionDetection = true,
            PersonDetection = true,
            IsExterior = true,
            IsInterior = true,
            Business = new Business()
        };
        _camera = new Camera
        {
            Name = _cameraRequest.Name,
            ModelNumber = _cameraRequest.ModelNumber,
            Description = _cameraRequest.Description,
            MainPhoto = _cameraRequest.MainPhoto,
            SecondaryPhotos = _cameraRequest.SecondaryPhotos,
            MotionDetection = _cameraRequest.MotionDetection,
            PersonDetection = _cameraRequest.PersonDetection,
            IsExterior = _cameraRequest.IsExterior,
            IsInterior = _cameraRequest.IsInterior,
            Business = _cameraRequest.Business
        };
    }

    #region CreateBusinessOwner
    [TestMethod]
    public void CreateBusinessOwner_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        _adminService.Setup(x => x.CreateBusinessOwner(_userModel)).Returns(_guid);

        // Act
        var response = _controller.CreateBusinessOwner(_businessOwnerRequest);

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Id.Should().Be(_guid.ToString());
    }
    #endregion

    #region CreateBusiness
    [TestMethod]
    public void CreateBusiness_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        _businessOwnerService.Setup(x => x.CreateBusiness(_business.Owner.Email, _business.Rut, _business.Name)).Returns(_business.Rut);

        // Act
        var response = _controller.CreateBusiness(_businessRequest, $"Bearer {_business.Owner.Id}");

        // Assert
        _businessOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.Id.Should().Be(_business.Rut);
    }
    #endregion

    #region CreateDevices
    [TestMethod]
    public void CreateDevice_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        _businessOwnerService.Setup(x => x.CreateDevice(_device.Name, _device.ModelNumber, _device.Description, _device.MainPhoto, _device.SecondaryPhotos, _device.Type, _device.Business)).Returns(_device.Id);

        // Act
        var response = _controller.CreateDevice(_deviceRequest, $"Bearer {_device.Id}");

        // Assert
        _businessOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.Id.Should().Be(_device.Id);
    }

    [TestMethod]
    public void CreateCamera_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        _businessOwnerService.Setup(x => x.CreateCamera(_camera.Name, _camera.ModelNumber, _camera.Description, _camera.MainPhoto, _camera.SecondaryPhotos, _camera.Business, _camera.MotionDetection, _camera.PersonDetection, _camera.IsExterior, _camera.IsInterior)).Returns(_camera.Id);

        // Act
        var response = _controller.CreateCamera(_cameraRequest, $"Bearer {_camera.Id}");

        // Assert
        _businessOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.Id.Should().Be(_camera.Id);
    }
    #endregion
}
