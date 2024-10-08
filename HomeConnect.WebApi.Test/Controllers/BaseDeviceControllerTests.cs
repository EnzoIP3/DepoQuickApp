using BusinessLogic.Devices.Services;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Devices;
using HomeConnect.WebApi.Controllers.Devices.Models;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class BaseDeviceControllerTests
{
    private BaseDeviceController _deviceController = null!;
    private Mock<IDeviceService> _deviceServiceMock = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _deviceServiceMock = new Mock<IDeviceService>();
        _deviceController = new BaseDeviceController(_deviceServiceMock.Object);
    }

    [TestMethod]
    public void Toggle_WithHardwareId_ReturnsConnectionResponse()
    {
        // Arrange
        var hardwareId = "hardwareId";
        _deviceServiceMock.Setup(x => x.ToggleDevice(hardwareId)).Returns(true);

        // Act
        ConnectionResponse result = _deviceController.Toggle(hardwareId);

        // Assert
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
        result.Connected.Should().BeTrue();
    }
}
