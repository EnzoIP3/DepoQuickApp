using BusinessLogic.Devices.Repositories;
using BusinessLogic.Devices.Services;
using FluentAssertions;
using Moq;

namespace HomeConnect.BusinessLogic.Test.Devices.Services;

[TestClass]
public class DeviceServiceTests
{
    private Mock<IOwnedDeviceRepository> _ownedDeviceRepositoryMock = null!;
    private DeviceService _deviceService = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _ownedDeviceRepositoryMock = new Mock<IOwnedDeviceRepository>();
        _deviceService = new DeviceService(_ownedDeviceRepositoryMock.Object);
    }

    [TestMethod]
    [DataRow("hardwareId")]
    [DataRow("")]
    public void Toggle_WhenHardwareIdIsInvalid_ShouldThrowArgumentException(string id)
    {
        // Act
        var act = () => _deviceService.Toggle(id);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Hardware ID is invalid");
    }

    [TestMethod]
    public void Toggle_WhenHardwareIdIsValid_ShouldReturnConnectionState()
    {
        // Arrange
        var hardwareId = Guid.NewGuid().ToString();
        _ownedDeviceRepositoryMock.Setup(x => x.Exists(hardwareId)).Returns(true);
        _ownedDeviceRepositoryMock.Setup(x => x.ToggleConnection(hardwareId)).Returns(true);

        // Act
        var result = _deviceService.Toggle(hardwareId);

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void Toggle_WhenOwnedDeviceDoesNotExist_ShouldThrowArgumentException()
    {
        // Arrange
        var hardwareId = Guid.NewGuid().ToString();
        _ownedDeviceRepositoryMock.Setup(x => x.Exists(hardwareId)).Returns(false);

        // Act
        var act = () => _deviceService.Toggle(hardwareId);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Owned device does not exist");
    }
}
