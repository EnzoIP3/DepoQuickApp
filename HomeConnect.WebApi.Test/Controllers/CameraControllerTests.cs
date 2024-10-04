using BusinessLogic.Devices.Services;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Camera;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class CameraControllerTests
{
    private Mock<IDeviceService> _deviceServiceMock = null!;
    private CameraController _cameraController = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _deviceServiceMock = new Mock<IDeviceService>();
        _cameraController = new CameraController();
    }

    [TestMethod]
    public void Toggle_WithHardwareId_ReturnsConnectionResponse()
    {
        // Arrange
        var hardwareId = "hardwareId";
        _deviceServiceMock.Setup(x => x.Toogle(hardwareId)).Returns(true);

        // Act
        var result = _cameraController.Toggle(hardwareId);

        // Assert
        result.Should().NotBeNull();
        result.HardwareId.Should().Be(hardwareId);
        result.ConnectionState.Should().BeTrue();
    }
}
