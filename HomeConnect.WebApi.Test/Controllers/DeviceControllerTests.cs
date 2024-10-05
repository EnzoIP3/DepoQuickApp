using System.Collections.Generic;
using BusinessLogic;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Device;
using HomeConnect.WebApi.Controllers.Devices.Models;
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
        var parameters = new DeviceQueryParameters();
        _deviceService.Setup(x => x.GetDevices(parameters)).Returns(_pagedList);

        // Act
        var response = _controller.GetDevices(parameters);

        // Assert
        _deviceService.Verify(x => x.GetDevices(parameters), Times.Once);
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
        var parameters = new DeviceQueryParameters { DeviceNameFilter = deviceNameFilter };
        _deviceService.Setup(x => x.GetDevices(parameters)).Returns(_pagedList);

        // Act
        var response = _controller.GetDevices(parameters);

        // Assert
        _deviceService.Verify(x => x.GetDevices(parameters), Times.Once);
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
        var parameters = new DeviceQueryParameters { ModelNumberFilter = modelNameFilter };
        _deviceService.Setup(x => x.GetDevices(parameters)).Returns(_pagedList);

        // Act
        var response = _controller.GetDevices(parameters);

        // Assert
        _deviceService.Verify(x => x.GetDevices(parameters), Times.Once);
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
        var parameters = new DeviceQueryParameters { BusinessNameFilter = businessNameFilter };
        _deviceService.Setup(x => x.GetDevices(parameters)).Returns(_pagedList);

        // Act
        var response = _controller.GetDevices(parameters);

        // Assert
        _deviceService.Verify(x => x.GetDevices(parameters), Times.Once);
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
        var parameters = new DeviceQueryParameters { DeviceTypeFilter = deviceTypeFilter };
        _deviceService.Setup(x => x.GetDevices(parameters)).Returns(_pagedList);

        // Act
        var response = _controller.GetDevices(parameters);

        // Assert
        _deviceService.Verify(x => x.GetDevices(parameters), Times.Once);
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(new { Data = _expectedDevices, Pagination = _expectedPagination });
    }
}
