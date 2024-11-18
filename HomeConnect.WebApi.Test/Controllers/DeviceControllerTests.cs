using BusinessLogic;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;
using BusinessLogic.Devices.Services;
using BusinessLogic.HomeOwners.Models;
using BusinessLogic.HomeOwners.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Devices;
using HomeConnect.WebApi.Controllers.Devices.Models;
using HomeConnect.WebApi.Controllers.HomeOwners.Models;
using HomeConnect.WebApi.Controllers.Homes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using GetDevicesResponse = HomeConnect.WebApi.Controllers.Devices.Models.GetDevicesResponse;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class DeviceControllerTests
{
    private DeviceController _controller = null!;
    private Device _device = null!;
    private Mock<IDeviceService> _deviceService = null!;
    private Mock<IValidatorService> _validatorService = null!;
    private Mock<IImporterService> _importerService = null!;
    private Mock<IHomeOwnerService> _homeOwnerService = null!;
    private Mock<HttpContext> _httpContextMock = null!;
    private List<Device> _expectedDevices = null!;
    private Pagination _expectedPagination = null!;
    private Device _otherDevice = null!;
    private PagedData<Device> _pagedList = null!;
    private User _user = null!;

    [TestInitialize]
    public void Initialize()
    {
        _deviceService = new Mock<IDeviceService>();
        _validatorService = new Mock<IValidatorService>();
        _homeOwnerService = new Mock<IHomeOwnerService>();
        _importerService = new Mock<IImporterService>();
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
        _controller =
            new DeviceController(_deviceService.Object, _validatorService.Object, _importerService.Object,
                _homeOwnerService.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = _httpContextMock.Object }
            };
        _user = new User("John", "Doe", "email@email.com", "Password@100",
            new Role { Name = "HomeOwner", Permissions = [] });

        var business = new Business("Business", "123456789", "https://www.example.com/logo.jpg", new User());
        _device = new Device("example1", "123", "example description 1", "https://www.example.com/photo1.jpg", [],
            "Sensor", business);
        _otherDevice = new Device("example1", "1234", "example description 2", "https://www.example.com/photo2.jpg", [],
            "Camera", business);
        _expectedDevices = [_device, _otherDevice];
        _expectedPagination = new Pagination { Page = 1, PageSize = 10, TotalPages = 1 };
        _pagedList = new PagedData<Device>
        {
            Data = _expectedDevices,
            Page = _expectedPagination.Page,
            PageSize = _expectedPagination.PageSize,
            TotalPages = _expectedPagination.TotalPages
        };
    }

    [TestMethod]
    public void GetDevices_WhenCalledWithValidRequestAndNoFiltersOrPagination_ReturnsExpectedResponse()
    {
        // Arrange
        var expectedResponse = new GetDevicesResponse
        {
            Devices = _expectedDevices.Select(d => new ListDeviceInfo
            {
                Id = d.Id.ToString(),
                Name = d.Name,
                BusinessName = d.Business.Name,
                Type = d.Type.ToString(),
                ModelNumber = d.ModelNumber,
                MainPhoto = d.MainPhoto,
                SecondaryPhotos = d.SecondaryPhotos,
                Description = d.Description
            }).ToList(),
            Pagination = _expectedPagination
        };
        _deviceService.Setup(x => x.GetDevices(It.IsAny<GetDevicesArgs>())).Returns(_pagedList);

        // Act
        GetDevicesResponse response = _controller.GetDevices(new GetDevicesRequest());

        // Assert
        _deviceService.Verify(x => x.GetDevices(It.IsAny<GetDevicesArgs>()), Times.Once);
        response.Should().BeEquivalentTo(expectedResponse);
    }

    #region ImportDevices

    [TestMethod]
    public void ImportDevices_WhenCalledWithValidImporterNameAndRoute_ReturnsImportDevicesResponse()
    {
        // Arrange
        var importerName = "Importer1";
        var route = "C:/Users/username/Documents/file.csv";
        var expectedDevices = new List<string> { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };
        var expectedResponse = new ImportDevicesResponse { ImportedDevices = expectedDevices };
        var request = new ImportDevicesRequest { ImporterName = importerName, Route = route };
        var userLoggedIn = new User("John", "Doe", "email@email.com", "Password@100",
            new Role { Name = "BusinessOwner", Permissions = [] });
        var items = new Dictionary<object, object?> { { Item.UserLogged, userLoggedIn } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        var args = new ImportDevicesArgs { ImporterName = importerName, FileName = route, User = userLoggedIn };
        _importerService.Setup(x => x.ImportDevices(args)).Returns(expectedDevices);

        // Act
        var response = _controller.ImportDevices(request);

        // Assert
        _importerService.Verify(x => x.ImportDevices(args), Times.Once);
        response.Should().BeEquivalentTo(expectedResponse, options => options
            .ComparingByMembers<ImportDevicesResponse>());
    }

    #endregion

    #region Turning

    [TestMethod]
    public void TurnOn_WithHardwareId_ReturnsConnectionResponse()
    {
        // Arrange
        var hardwareId = "hardwareId";
        _deviceService.Setup(x => x.TurnDevice(hardwareId, true)).Returns(true);

        // Act
        ConnectionResponse result = _controller.TurnOn(hardwareId);

        // Assert
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
        result.Connected.Should().BeTrue();
    }

    [TestMethod]
    public void TurnOff_WithHardwareId_ReturnsConnectionResponse()
    {
        // Arrange
        var hardwareId = "hardwareId";
        _deviceService.Setup(x => x.TurnDevice(hardwareId, false)).Returns(false);

        // Act
        ConnectionResponse result = _controller.TurnOff(hardwareId);

        // Assert
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
        result.Connected.Should().BeFalse();
    }

    #endregion

    #region NameDevice

    #region Success

    [TestMethod]
    public void NameDevice_WithValidRequest_ReturnsDeviceId()
    {
        // Arrange
        var hardwareId = Guid.NewGuid().ToString();
        var request = new NameDeviceRequest { NewName = "New Device Name" };
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        var args = new NameDeviceArgs
        {
            HardwareId = Guid.Parse(hardwareId),
            NewName = request.NewName,
            OwnerId = _user.Id
        };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _homeOwnerService.Setup(x => x.NameDevice(args));

        // Act
        NameDeviceResponse response = _controller.NameDevice(hardwareId, request);

        // Assert
        _homeOwnerService.Verify(x => x.NameDevice(args),
            Times.Once);
        response.Should().NotBeNull();
        response.DeviceId.Should().Be(hardwareId);
    }

    #endregion

    #region Error

    [TestMethod]
    public void NameDevice_WithInvalidNewName_ThrowsArgumentException()
    {
        // Arrange
        var hardwareId = Guid.NewGuid().ToString();
        var request = new NameDeviceRequest { NewName = string.Empty };
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);

        // Act & Assert
        var ex = Assert.ThrowsException<ArgumentException>(() => _controller.NameDevice(hardwareId, request));
        Assert.AreEqual("NewName cannot be null or empty", ex.Message);
    }

    [TestMethod]
    public void NameDevice_WithInvalidDeviceId_ThrowsArgumentException()
    {
        // Arrange
        var hardwareId = string.Empty;
        var request = new NameDeviceRequest { NewName = "New Device Name" };
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);

        // Act & Assert
        var ex = Assert.ThrowsException<ArgumentException>(() => _controller.NameDevice(hardwareId, request));
        Assert.AreEqual("DeviceId cannot be null or empty", ex.Message);
    }

    #endregion

    #endregion

    [TestMethod]
    public void MoveDevice_WhenCalledWithValidRequest_ReturnsExpectedResponse()
    {
        // Arrange
        var targetRoomId = "123e4567-e89b-12d3-a456-426614174001";
        var deviceId = "123e4567-e89b-12d3-a456-426614174002";

        _deviceService.Setup(x => x.MoveDevice(targetRoomId, deviceId)).Verifiable();

        var request = new MoveDeviceRequest
        {
            TargetRoomId = targetRoomId
        };

        // Act
        var response = _controller.MoveDevice(deviceId, request);

        // Assert
        _deviceService.VerifyAll();
        response.Should().NotBeNull();
        response.TargetRoomId.Should().Be(targetRoomId);
        response.DeviceId.Should().Be(deviceId);
    }
}
