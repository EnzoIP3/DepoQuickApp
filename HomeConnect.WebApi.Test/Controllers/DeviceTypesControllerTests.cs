using BusinessLogic;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;
using BusinessLogic.Devices.Services;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Device;
using HomeConnect.WebApi.Controllers.DeviceTypes;
using HomeConnect.WebApi.Controllers.DeviceTypes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class DeviceTypesControllerTests
{
    private DeviceTypesController _controller = null!;
    private Mock<IDeviceService> _deviceService = null!;
    private Mock<HttpContext> _httpContextMock = null!;
    private readonly string _defaultCacheTime = "2592000";

    [TestInitialize]
    public void Initialize()
    {
        _httpContextMock = new Mock<HttpContext>();
        _deviceService = new Mock<IDeviceService>();
        _controller = new DeviceTypesController(_deviceService.Object);
        _controller.ControllerContext = new ControllerContext { HttpContext = _httpContextMock.Object };
    }

    [TestMethod]
    public void GetDeviceTypes_WhenCalled_ReturnsDeviceTypesCachedResponse()
    {
        // Arrange
        var expectedDeviceTypes = new List<string> { "Sensor", "Camera" };
        _deviceService.Setup(x => x.GetAllDeviceTypes()).Returns(expectedDeviceTypes);
        _httpContextMock.Setup(x => x.Response.Headers).Returns(new HeaderDictionary());

        // Act
        var response = _controller.GetDeviceTypes();

        // Assert
        response.Should().NotBeNull();
        _deviceService.Verify(x => x.GetAllDeviceTypes(), Times.Once);
        _controller.Response.Headers["Cache-Control"].Should().BeEquivalentTo($"public,max-age={_defaultCacheTime}");
        response.DeviceTypes.Should().BeEquivalentTo(expectedDeviceTypes);
    }
}
