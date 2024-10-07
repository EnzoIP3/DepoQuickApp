using BusinessLogic.Devices.Services;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Device;
using HomeConnect.WebApi.Controllers.Device.Models;
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
        _deviceServiceMock.Setup(x => x.Toggle(hardwareId)).Returns(true);

        // Act
        ConnectionResponse result = _deviceController.Toggle(hardwareId);

        // Assert
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
        result.Connected.Should().BeTrue();
    }
}
