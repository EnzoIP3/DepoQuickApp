using System.Collections.Generic;
using BusinessLogic;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;
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
    private GetDevicesArgs _deviceArgs;
    private GetDevicesArgs _otherDeviceArgs;
    private List<GetDevicesArgs> _expectedDevicesArgs = null!;
    private Pagination _expectedPagination;
    private PagedData<GetDevicesArgs> _pagedList;

    [TestInitialize]
    public void Initialize()
    {
        _deviceService = new Mock<IDeviceService>();
        _controller = new DeviceController(_deviceService.Object);

        _deviceArgs = new GetDevicesArgs()
        {
            Name = "Device1",
            ModelNumber = 123,
            Description = "Test device",
            MainPhoto = "https://www.example.com/photo1.jpg",
            Type = "Camera",
            BusinessName = "Business1",
            OwnerEmail = "owner1@example.com"
        };
        _otherDeviceArgs = new GetDevicesArgs()
        {
            Name = "Device2",
            ModelNumber = 1234,
            Description = "Test device",
            MainPhoto = "https://www.example.com/photo2.jpg",
            Type = "Camera",
            BusinessName = "Business2",
            OwnerEmail = "owner2@example.com"
        };
        _expectedDevicesArgs = new List<GetDevicesArgs> { _deviceArgs, _otherDeviceArgs };
        _expectedPagination = new Pagination { Page = 1, PageSize = 10, TotalPages = 1 };
        _pagedList = new PagedData<GetDevicesArgs>
        {
            Data = _expectedDevicesArgs,
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
        var response = _controller.GetDevices();

        // Assert
        _deviceService.Verify(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), null, null, null, null), Times.Once);
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(new { Data = _expectedDevicesArgs, Pagination = _expectedPagination });
    }

    [TestMethod]
    public void GetDevices_WhenCalledWithValidRequestAndNameFilter_ReturnsFilteredExpectedResponse()
    {
        // Arrange
        var deviceNameFilter = _expectedDevicesArgs.First().Name;
        _deviceService.Setup(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), deviceNameFilter, null, null, null)).Returns(_pagedList);

        // Act
        var response = _controller.GetDevices(deviceNameFilter: deviceNameFilter);

        // Assert
        _deviceService.Verify(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), deviceNameFilter, null, null, null), Times.Once);
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(new { Data = _expectedDevicesArgs, Pagination = _expectedPagination });
    }

    [TestMethod]
    public void GetDevices_WhenCalledWithValidRequestAndModelNumberFilter_ReturnsFilteredExpectedResponse()
    {
        // Arrange
        var modelNameFilter = _expectedDevicesArgs.First().ModelNumber;
        _deviceService.Setup(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), null, modelNameFilter, null, null)).Returns(_pagedList);

        // Act
        var response = _controller.GetDevices(modelNameFilter: modelNameFilter);

        // Assert
        _deviceService.Verify(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), null, modelNameFilter, null, null), Times.Once);
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(new { Data = _expectedDevicesArgs, Pagination = _expectedPagination });
    }

    [TestMethod]
    public void GetDevices_WhenCalledWithValidRequestAndBusinessNameFilter_ReturnsFilteredExpectedResponse()
    {
        // Arrange
        var businessNameFilter = _expectedDevicesArgs.First().BusinessName;
        _deviceService.Setup(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), null, null, businessNameFilter, null)).Returns(_pagedList);

        // Act
        var response = _controller.GetDevices(businessNameFilter: businessNameFilter);

        // Assert
        _deviceService.Verify(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), null, null, businessNameFilter, null), Times.Once);
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(new { Data = _expectedDevicesArgs, Pagination = _expectedPagination });
    }

    [TestMethod]
    public void GetDevices_WhenCalledWithValidRequestAndDeviceTypeFilter_ReturnsFilteredExpectedResponse()
    {
        // Arrange
        var deviceTypeFilter = _expectedDevicesArgs.First().Type;
        _deviceService.Setup(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<string?>(), deviceTypeFilter)).Returns(_pagedList);

        // Act
        var response = _controller.GetDevices(deviceTypeFilter: deviceTypeFilter);

        // Assert
        _deviceService.Verify(x => x.GetDevices(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<string?>(), deviceTypeFilter), Times.Once);
        response.Should().NotBeNull();
        response.Should().BeOfType<OkObjectResult>();
        var okResult = response as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(new { Data = _expectedDevicesArgs, Pagination = _expectedPagination });
    }
}
