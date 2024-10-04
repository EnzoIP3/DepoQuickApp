using System.Collections.Generic;
using BusinessLogic;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Device;
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

        _device = new Device()
        {
            Name = "Device1",
            ModelNumber = 123,
            Description = "Test device",
            MainPhoto = "https://www.example.com/photo1.jpg",
            SecondaryPhotos = [],
            Type = "Camera",
            Business = new Business()
        };
        _otherDevice = new Device()
        {
            Name = "Device2",
            ModelNumber = 1234,
            Description = "Test device",
            MainPhoto = "https://www.example.com/photo2.jpg",
            SecondaryPhotos = [],
            Type = "Camera",
            Business = new Business()
        };
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
        _deviceService.Setup(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), null, null)).Returns(_pagedList);

        // Act
        var response = _controller.GetDevices();

        // Assert
        _deviceService.Verify(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), null, null), Times.Once);
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
        _deviceService.Setup(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), deviceNameFilter, null)).Returns(_pagedList);

        // Act
        var response = _controller.GetDevices(deviceNameFilter: deviceNameFilter);

        // Assert
        _deviceService.Verify(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), deviceNameFilter, null), Times.Once);
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
        _deviceService.Setup(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), null, modelNameFilter)).Returns(_pagedList);

        // Act
        var response = _controller.GetDevices(modelNameFilter: modelNameFilter);

        // Assert
        _deviceService.Verify(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), null, modelNameFilter), Times.Once);
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
        _deviceService.Setup(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), null, null, businessNameFilter)).Returns(_pagedList);

        // Act
        var response = _controller.GetDevices(businessNameFilter: businessNameFilter);

        // Assert
        _deviceService.Verify(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), null, null, businessNameFilter), Times.Once);
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(new { Data = _expectedDevices, Pagination = _expectedPagination });
    }
}
