using BusinessLogic;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.Devices.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using Moq;

namespace HomeConnect.BusinessLogic.Test.Devices.Services;

[TestClass]
public class DeviceServiceTests
{
    private Mock<IOwnedDeviceRepository> _ownedDeviceRepository = null!;
    private Mock<IDeviceRepository> _deviceRepository = null!;
    private DeviceService _deviceService = null!;
    private List<Device> _devices = null!;
    private PagedData<Device> _pagedDeviceList;
    private GetDeviceArgs _parameters = null!;
    private User user1 = null!;
    private User user2 = null!;
    private Device otherDevice = null!;
    private Device validDevice = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _deviceRepository = new Mock<IDeviceRepository>();
        _ownedDeviceRepository = new Mock<IOwnedDeviceRepository>();
        _deviceService = new DeviceService(_deviceRepository.Object, _ownedDeviceRepository.Object);

        user1 = new User("name", "surname", "email1@email.com", "Password#100", new Role());
        user2 = new User("name", "surname", "email2@email.com", "Password#100", new Role());
        validDevice = new Device("Device1", 12345, "Device description1", "https://example1.com/image.png",
            [], "Sensor", new Business("Rut1", "Business", user1));
        otherDevice = new Device("Device2", 12345, "Device description2", "https://example2.com/image.png",
            [], "Sensor", new Business("Rut2", "Business", user2));

        _devices = [validDevice, otherDevice];

        _parameters = new GetDeviceArgs
        {
            Page = 1,
            PageSize = 10
        };

        _pagedDeviceList = new PagedData<Device>
        {
            Data = _devices,
            Page = _parameters.Page ?? 1,
            PageSize = _parameters.PageSize ?? 10,
            TotalPages = 1
        };
    }

    [TestMethod]
    [DataRow("hardwareId")]
    [DataRow("")]
    public void Toggle_WhenHardwareIdIsInvalid_ShouldThrowArgumentException(string id)
    {
        // Act
        var act = () => _deviceService.Toggle(id);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Hardware ID is invalid.");
    }

    [TestMethod]
    public void Toggle_WhenHardwareIdIsValid_ShouldReturnConnectionState()
    {
        // Arrange
        var hardwareId = Guid.NewGuid().ToString();
        _ownedDeviceRepository.Setup(x => x.Exists(hardwareId)).Returns(true);
        _ownedDeviceRepository.Setup(x => x.ToggleConnection(hardwareId)).Returns(true);

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
        _ownedDeviceRepository.Setup(x => x.Exists(hardwareId)).Returns(false);

        // Act
        var act = () => _deviceService.Toggle(hardwareId);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Owned device does not exist.");
    }

    [TestMethod]
    public void GetDevices_WhenCalled_ReturnsDeviceList()
    {
        // Arrange
        _deviceRepository.Setup(x => x.GetDevices(_parameters.Page ?? 1, _parameters.PageSize ?? 10, _parameters.DeviceNameFilter, _parameters.ModelNumberFilter, _parameters.BusinessNameFilter, _parameters.DeviceTypeFilter)).Returns(_pagedDeviceList);

        // Act
        var result = _deviceService.GetDevices(_parameters);

        // Assert
        var expectedPagedDeviceList = new PagedData<Device>
        {
            Data = _devices,
            Page = _parameters.Page ?? 1,
            PageSize = _parameters.PageSize ?? 10,
            TotalPages = 1
        };

        result.Should().BeEquivalentTo(expectedPagedDeviceList, options => options.ComparingByMembers<PagedData<Device>>());
        _deviceRepository.Verify(x => x.GetDevices(
            It.Is<int>(a => a == _parameters.Page),
            It.Is<int>(a => a == _parameters.PageSize),
            It.Is<string>(a => a == _parameters.DeviceNameFilter),
            It.Is<int?>(a => a == _parameters.ModelNumberFilter),
            It.Is<string>(a => a == _parameters.BusinessNameFilter),
            It.Is<string>(a => a == _parameters.DeviceTypeFilter)), Times.Once);
    }

    [TestMethod]
    public void GetAllDeviceTypes_WhenCalled_ReturnsDeviceTypes()
    {
        // Arrange
        var expectedDeviceTypes = new List<string> { "Sensor", "Camera" };
        _deviceRepository.Setup(x => x.GetDevices(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new PagedData<Device>
        {
            Data =
            [
                new Device { Type = DeviceType.Sensor },
                new Device { Type = DeviceType.Camera },
                new Device { Type = DeviceType.Sensor }
            ],
            Page = 1,
            PageSize = 3,
            TotalPages = 1
        });

        // Act
        var result = _deviceService.GetAllDeviceTypes();

        // Assert
        _deviceRepository.Verify(x => x.GetDevices(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        result.Should().BeEquivalentTo(expectedDeviceTypes);
    }
}
