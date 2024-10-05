using System.Collections.Generic;
using BusinessLogic;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Device;
using HomeConnect.WebApi.Controllers.Device.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class DeviceControllerTests
{
    private DeviceController _controller = null!;
    private Mock<IDeviceService> _deviceService = null!;
    private Device _device = null!;
    private Device _otherDevice = null!;
    private List<Device> _expectedDevices = null!;
    private Pagination _expectedPagination;
    private PagedData<Device> _pagedList;

    [TestInitialize]
    public void Initialize()
    {
        _deviceService = new Mock<IDeviceService>();
        _controller = new DeviceController(_deviceService.Object);

        _device = new Device("example1", 123, "example description 1", "https://www.example.com/photo1.jpg", [], "Sensor", new Business());
        _otherDevice = new Device("example1", 1234, "example description 2", "https://www.example.com/photo2.jpg", [], "Camera", new Business());
        _expectedDevices = new List<Device> { _device, _otherDevice };
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
        _deviceService.Setup(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), null, null, null, null)).Returns(_pagedList);

        // Act
        var response = _controller.GetDevices(new DeviceQueryParameters());

        // Assert
        _deviceService.Verify(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), null, null, null, null), Times.Once);
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(new { Data = _expectedDevices, Pagination = _expectedPagination });
    }

    [TestMethod]
    public void GetDevices_WhenCalledWithValidRequestAndNameFilter_ReturnsFilteredExpectedResponse()
    {
        // Arrange
        var deviceNameFilter = _expectedDevices.First().Name;
        _deviceService.Setup(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), deviceNameFilter, null, null, null)).Returns(_pagedList);

        // Act
        var response = _controller.GetDevices(new DeviceQueryParameters { DeviceNameFilter = deviceNameFilter });

        // Assert
        _deviceService.Verify(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), deviceNameFilter, null, null, null), Times.Once);
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(new { Data = _expectedDevices, Pagination = _expectedPagination });
    }

    [TestMethod]
    public void GetDevices_WhenCalledWithValidRequestAndModelNumberFilter_ReturnsFilteredExpectedResponse()
    {
        // Arrange
        var modelNameFilter = _expectedDevices.First().ModelNumber;
        _deviceService.Setup(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), null, modelNameFilter, null, null)).Returns(_pagedList);

        // Act
        var response = _controller.GetDevices(new DeviceQueryParameters { ModelNameFilter = modelNameFilter });

        // Assert
        _deviceService.Verify(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), null, modelNameFilter, null, null), Times.Once);
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(new { Data = _expectedDevices, Pagination = _expectedPagination });
    }

    [TestMethod]
    public void GetDevices_WhenCalledWithValidRequestAndBusinessNameFilter_ReturnsFilteredExpectedResponse()
    {
        // Arrange
        var businessNameFilter = _expectedDevices.First().Business.Name;
        _deviceService.Setup(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), null, null, businessNameFilter, null)).Returns(_pagedList);

        // Act
        var response = _controller.GetDevices(new DeviceQueryParameters { BusinessNameFilter = businessNameFilter });

        // Assert
        _deviceService.Verify(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), null, null, businessNameFilter, null), Times.Once);
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(new { Data = _expectedDevices, Pagination = _expectedPagination });
    }

    [TestMethod]
    public void GetDevices_WhenCalledWithValidRequestAndDeviceTypeFilter_ReturnsFilteredExpectedResponse()
    {
        // Arrange
        var deviceTypeFilter = _expectedDevices.First().Type;
        _deviceService.Setup(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<string?>(), deviceTypeFilter)).Returns(_pagedList);

        // Act
        var response = _controller.GetDevices(new DeviceQueryParameters { DeviceTypeFilter = deviceTypeFilter });

        // Assert
        _deviceService.Verify(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<string?>(), deviceTypeFilter), Times.Once);
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(new { Data = _expectedDevices, Pagination = _expectedPagination });
    }
}
