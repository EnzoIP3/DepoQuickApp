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
        _cameraController = new CameraController(_deviceServiceMock.Object);
    }
}
