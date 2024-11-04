using BusinessLogic;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;
using BusinessLogic.Devices.Services;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Devices;
using HomeConnect.WebApi.Controllers.Devices.Models;
using HomeConnect.WebApi.Controllers.Homes.Models;
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
        _controller = new DeviceController(_deviceService.Object, _validatorService.Object, _importerService.Object);

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

    #region GetImporters
    [TestMethod]
    public void GetImporters_WhenCalled_ReturnsGetImportersResponse()
    {
        // Arrange
        var importers = new List<string>
        {
            "Importer1",
            "Importer2"
        };
        var expectedResponse = new GetImportersResponse
        {
            Importers = importers
        };
        _importerService.Setup(x => x.GetImporters()).Returns(importers);

        // Act
        GetImportersResponse response = _controller.GetImporters();

        // Assert
        _importerService.Verify(x => x.GetImporters(), Times.Once);
        response.Should().BeEquivalentTo(expectedResponse, options => options
            .ComparingByMembers<GetImportersResponse>());
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
        _importerService.Setup(x => x.ImportDevices(importerName, route)).Returns(expectedDevices);

        // Act
        var response = _controller.ImportDevices(importerName, route);

        // Assert
        _importerService.Verify(x => x.ImportDevices(importerName, route), Times.Once);
        response.Should().BeEquivalentTo(expectedResponse, options => options
            .ComparingByMembers<ImportDevicesResponse>());
    }
    #endregion
}
