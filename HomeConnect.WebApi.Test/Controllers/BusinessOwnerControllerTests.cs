using BusinessLogic.Admins.Services;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Models;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Businesses;
using HomeConnect.WebApi.Controllers.Businesses.Models;
using HomeConnect.WebApi.Controllers.BusinessOwner;
using HomeConnect.WebApi.Controllers.BusinessOwner.Models;
using HomeConnect.WebApi.Controllers.Camera.Models;
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
    private CreateDeviceArgs _deviceArgs;
    private Device _device = null!;
    private CreateCameraRequest _cameraRequest;

    [TestInitialize]
    public void Initialize()
    {
        _adminService = new Mock<IAdminService>();
        _businessOwnerService = new Mock<IBusinessOwnerService>();
        _controller = new BusinessOwnerController(_adminService.Object, _businessOwnerService.Object);

        _businessOwnerRequest = new CreateBusinessOwnerRequest
        {
            Name = "John", Surname = "Doe", Email = "email@email.com", Password = "password"
        };
        _userModel = new CreateUserArgs
        {
            Name = _businessOwnerRequest.Name,
            Surname = _businessOwnerRequest.Surname,
            Email = _businessOwnerRequest.Email,
            Password = _businessOwnerRequest.Password
        };
        _guid = Guid.NewGuid();
        var user = new User();
        _businessRequest = new CreateBusinessRequest { Name = "OSE", Rut = "306869575", OwnerId = user.Id.ToString() };
        _business = new Business { Name = _businessRequest.Name, Rut = _businessRequest.Rut, Owner = user };
        _deviceRequest = new CreateDeviceRequest
        {
            Name = "Device1",
            ModelNumber = 123,
            Description = "Test device",
            MainPhoto = "https://www.example.com/photo1.jpg",
            SecondaryPhotos = new List<string>(),
            Type = "Camera",
            BusinessRut = "306869575"
        };
        _deviceArgs = new CreateDeviceArgs
        {
            Name = _deviceRequest.Name,
            ModelNumber = _deviceRequest.ModelNumber,
            Description = _deviceRequest.Description,
            MainPhoto = _deviceRequest.MainPhoto,
            SecondaryPhotos = _deviceRequest.SecondaryPhotos,
            Type = _deviceRequest.Type,
            BusinessRut = _deviceRequest.BusinessRut
        };
        _device = new Device
        {
            Name = _deviceRequest.Name,
            ModelNumber = _deviceRequest.ModelNumber,
            Description = _deviceRequest.Description,
            MainPhoto = _deviceRequest.MainPhoto,
            SecondaryPhotos = _deviceRequest.SecondaryPhotos,
            Type = _deviceRequest.Type,
            Business = _business
        };
        _cameraRequest = new CreateCameraRequest
        {
            Name = "Camera1",
            ModelNumber = 123,
            Description = "Test camera",
            MainPhoto = "https://www.example.com/photo1.jpg",
            SecondaryPhotos = new List<string>(),
            MotionDetection = true,
            PersonDetection = true,
            IsExterior = true,
            IsInterior = true,
            BusinessRut = "306869575"
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

    #region CreateDevices

    [TestMethod]
    public void CreateDevice_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        _businessOwnerService.Setup(x => x.CreateDevice(_deviceArgs)).Returns(_device.Id);

        // Act
        var response = _controller.CreateDevice(_deviceRequest);

        // Assert
        _businessOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.Id.Should().Be(_device.Id);
    }

    #endregion
}
