using BusinessLogic.Devices.Services;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test.Devices.Services;

[TestClass]
public class DeviceServiceTests
{
    private DeviceService _deviceService = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _deviceService = new DeviceService();
    }

    [TestMethod]
    [DataRow("hardwareId")]
    [DataRow("")]
    public void Toogle_WhenHardwareIdIsInvalid_ShouldThrowArgumentException(string id)
    {
        // Act
        var act = () => _deviceService.Toogle(id);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Hardware ID is invalid");
    }
}
