using BusinessLogic;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;
using BusinessLogic.Devices.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Devices;
using HomeConnect.WebApi.Controllers.Devices.Models;
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
    private Mock<HttpContext> _httpContextMock = null!;
    private List<Device> _expectedDevices = null!;
    private Pagination _expectedPagination = null!;
    private Device _otherDevice = null!;
    private PagedData<Device> _pagedList = null!;

    [TestInitialize]
    public void Initialize()
    {
        _deviceService = new Mock<IDeviceService>();
        _validatorService = new Mock<IValidatorService>();
        _importerService = new Mock<IImporterService>();
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
        _controller = new DeviceController(_deviceService.Object, _validatorService.Object, _importerService.Object)
        {
            ControllerContext = new ControllerContext { HttpContext = _httpContextMock.Object }
        };

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
                HardwareId = d.Id.ToString(),
                Name = d.Name,
                BusinessName = d.Business.Name,
                Type = d.Type.ToString(),
                ModelNumber = d.ModelNumber,
                Photo = d.MainPhoto
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

    #region GetValidators
    [TestMethod]
    public void GetValidators_WhenCalled_ReturnsGetValidatorsResponse()
    {
        // Arrange
        var validators = new List<ValidatorInfo>
        {
            new ValidatorInfo { Name = "Validator1" },
            new ValidatorInfo { Name = "Validator2" }
        };
        var expectedResponse = new GetValidatorsResponse
        {
            Validators = validators.Select(v => v.Name).ToList()
        };
        _validatorService.Setup(x => x.GetValidators()).Returns(validators);

        // Act
        GetValidatorsResponse response = _controller.GetValidators();

        // Assert
        _validatorService.Verify(x => x.GetValidators(), Times.Once);
        response.Should().BeEquivalentTo(expectedResponse, options => options
            .ComparingByMembers<GetValidatorsResponse>());
    }
    #endregion

    #region ImportDevices
    [TestMethod]
    public void ImportDevices_WhenCalledWithValidImporterNameAndRoute_ReturnsImportDevicesResponse()
    {
        // Arrange
        var importerName = "Importer1";
        var route = "C:/Users/username/Documents/file.csv";
        var expectedDevices = new List<string>{ Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };
        var expectedResponse = new ImportDevicesResponse
        {
            ImportedDevices = expectedDevices
        };
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
    #region GetImportFiles
    [TestMethod]
    public void GetImportFiles_WhenCalled_ReturnsGetImportFilesResponse()
    {
        // Arrange
        var files = new List<string>
        {
            "file1.csv",
            "file2.json"
        };
        var expectedResponse = new GetImportFilesResponse
        {
            ImportFiles = files
        };
        _importerService.Setup(x => x.GetImportFiles()).Returns(files);

        // Act
        GetImportFilesResponse response = _controller.GetImportFiles();

        // Assert
        _importerService.Verify(x => x.GetImportFiles(), Times.Once);
        response.Should().BeEquivalentTo(expectedResponse, options => options
            .ComparingByMembers<GetImportFilesResponse>());
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
}
