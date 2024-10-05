using BusinessLogic.Devices.Services;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Device;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class BaseDeviceControllerTests
{
    private Mock<IDeviceService> _deviceServiceMock = null!;
    private BaseDeviceController _deviceController = null!;

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
        var result = _deviceController.Toggle(hardwareId);

        // Assert
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
        result.ConnectionState.Should().BeTrue();
    }
}
