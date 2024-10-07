using BusinessLogic;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;
using BusinessLogic.Devices.Services;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Device;
using HomeConnect.WebApi.Controllers.Device.Models;
using HomeConnect.WebApi.Controllers.Home.Models;
using Moq;
using GetDevicesResponse = HomeConnect.WebApi.Controllers.Device.Models.GetDevicesResponse;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class DeviceControllerTests
{
    private DeviceController _controller = null!;
    private Device _device = null!;
    private Mock<IDeviceService> _deviceService = null!;
    private List<Device> _expectedDevices = null!;
    private Pagination _expectedPagination = null!;
    private Device _otherDevice = null!;
    private PagedData<Device> _pagedList = null!;

    [TestInitialize]
    public void Initialize()
    {
        _deviceService = new Mock<IDeviceService>();
        _controller = new DeviceController(_deviceService.Object);

        var business = new Business("Business", "123456789", "https://www.example.com/logo.jpg", new User());
        _device = new Device("example1", 123, "example description 1", "https://www.example.com/photo1.jpg", [],
            "Sensor", business);
        _otherDevice = new Device("example1", 1234, "example description 2", "https://www.example.com/photo2.jpg", [],
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
                Photo = d.MainPhoto,
                IsConnected = d.ConnectionState
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
}
