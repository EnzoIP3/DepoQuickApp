using BusinessLogic;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;
using BusinessLogic.Devices.Services;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Device;
using HomeConnect.WebApi.Controllers.DeviceTypes;
using HomeConnect.WebApi.Controllers.DeviceTypes.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class DeviceTypesControllerTests
{
    private DeviceTypesController _controller = null!;
    private Mock<IDeviceService> _deviceService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _deviceService = new Mock<IDeviceService>();
        _controller = new DeviceTypesController(_deviceService.Object);
    }

    [TestMethod]
    public void GetDeviceTypes_WhenCalled_ReturnsExpectedResponse()
    {
        // Arrange
        var expectedDeviceTypes = new List<string> { "Sensor", "Camera" };
        _deviceService.Setup(x => x.GetAllDeviceTypes()).Returns(expectedDeviceTypes);

        // Act
        var response = _controller.GetDeviceTypes();

        // Assert
        _deviceService.Verify(x => x.GetAllDeviceTypes(), Times.Once);
        response.Should().BeOfType<GetDeviceTypesResponse>();
        response.DeviceTypes.Should().BeEquivalentTo(expectedDeviceTypes);
    }
}
