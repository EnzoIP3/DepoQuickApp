using BusinessLogic;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.DataAccess.Repositories;

namespace HomeConnect.DataAccess.Test.Repositories;

[TestClass]
public class DeviceRepositoryTests
{
    private readonly Context _context = DbContextBuilder.BuildTestDbContext();
    private DeviceRepository _deviceRepository = null!;
    private Role _role = null!;
    private Device _secondValidDevice = null!;
    private Device _validDevice = null!;
    private User _validUser = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();
        _deviceRepository = new DeviceRepository(_context);
        _role = new Role();
        _validUser = new User("John", "Doe", "johhnDoe@example.com", "Password#100", _role);
        _validDevice = new Device("DeviceValid", "123456", "Device description", "https://example.com/image.png",
            [], "Camera", new Business("123456", "BusinessValid", "https://example.com/image.png", _validUser));
        _secondValidDevice = new Device("DeviceValid2", "1234567", "Device description",
            "https://example2.com/image.png",
            [], "Sensor", new Business("1234567", "BusinessValid2", "https://example.com/image.png", _validUser));
        _context.Add(_validDevice);
        _context.Add(_secondValidDevice);
        _context.SaveChanges();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    #region Add

    #region Success

    [TestMethod]
    public void Add_WhenDeviceDoesNotExist_AddsDevice()
    {
        // Arrange
        var business = new Business("12345", "Business", "https://example.com/image.png", _validUser);
        var device = new Device("Device", "12345", "Device description", "https://example.com/image.png",
            [], "Sensor", business);

        // Act
        _deviceRepository.Add(device);

        // Assert
        _context.Devices.Should().Contain(device);
    }

    #endregion

    #endregion

    #region ExistsByModelNumber

    #region Success

    [TestMethod]
    public void ExistsByModelNumber_WhenDeviceExists_ReturnsTrue()
    {
        // Arrange
        var device = new Device("Device", "12345", "Device description", "https://example.com/image.png",
            [], "Sensor", new Business("12345", "Business", "https://example.com/image.png", _validUser));
        _deviceRepository.Add(device);

        // Act
        var result = _deviceRepository.ExistsByModelNumber(device.ModelNumber!);

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #endregion

    #region Get

    #region Success

    [TestMethod]
    public void Get_WhenDeviceExists_ReturnsDevice()
    {
        // Arrange
        var business = new Business("12345", "Business", "https://example.com/image.png", _validUser);
        var device = new Device("Device", "12345", "Device description", "https://example.com/image.png",
            [], "Sensor", business);
        _deviceRepository.Add(device);
        _context.SaveChanges();

        // Act
        Device result = _deviceRepository.Get(device.Id);

        // Assert
        result.Should().BeEquivalentTo(device);
    }

    #endregion

    #endregion

    #region Exists

    [TestMethod]
    public void Exists_WhenDeviceExists_ReturnsTrue()
    {
        // Arrange
        var device = new Device("Device", "12345", "Device description", "https://example.com/image.png",
            [], "Sensor", new Business("12345", "Business", "https://example.com/image.png", _validUser));
        _deviceRepository.Add(device);

        // Act
        var result = _deviceRepository.Exists(device.Id);

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #region GetDevices

    #region Success

    [TestMethod]
    public void GetDevices_WhenCalled_ReturnsPaginatedDevices()
    {
        // Arrange
        var args = new GetDevicesArgs { Page = 1, PageSize = 2 };

        // Act
        PagedData<Device> result = _deviceRepository.GetPaged(args);

        // Assert
        result.Data.Should().HaveCount(2);
        result.Data.Exists(d => d.Id == _validDevice.Id).Should().BeTrue();
        result.Data.Exists(d => d.Id == _secondValidDevice.Id).Should().BeTrue();
    }

    [TestMethod]
    public void GetDevices_WhenFilteredByDeviceName_ReturnsFilteredDevices()
    {
        // Arrange
        var deviceNameFilter = "DeviceValid";
        var args = new GetDevicesArgs { Page = 1, PageSize = 10, DeviceNameFilter = deviceNameFilter };

        // Act
        PagedData<Device> result = _deviceRepository.GetPaged(args);

        // Assert
        result.Data.Should().HaveCount(2);
        result.Data.All(d => d.Name.Contains(deviceNameFilter)).Should().BeTrue();
    }

    [TestMethod]
    public void GetDevices_WhenFilteredByModelNumber_ReturnsFilteredDevices()
    {
        // Arrange
        var modelNumberFilter = "1234567";
        var args = new GetDevicesArgs { Page = 1, PageSize = 10, ModelNumberFilter = modelNumberFilter };

        // Act
        PagedData<Device> result = _deviceRepository.GetPaged(args);

        // Assert
        result.Data.Should().HaveCount(1);
        result.Data.First().ModelNumber.Should().Be(modelNumberFilter);
    }

    [TestMethod]
    public void GetDevices_WhenFilteredByBusinessName_ReturnsFilteredDevices()
    {
        // Arrange
        var businessNameFilter = "BusinessValid2";
        var args = new GetDevicesArgs { Page = 1, PageSize = 10, BusinessNameFilter = businessNameFilter };

        // Act
        PagedData<Device> result = _deviceRepository.GetPaged(args);

        // Assert
        result.Data.Should().HaveCount(1);
        result.Data.First().Business.Name.Should().Contain(businessNameFilter);
    }

    [TestMethod]
    [DataRow("Camera")]
    [DataRow("Sensor")]
    public void GetDevices_WhenFilteredByDeviceType_ReturnsFilteredDevices(string deviceTypeFilter)
    {
        // Arrange
        DeviceType deviceType = Enum.Parse<DeviceType>(deviceTypeFilter);
        var args = new GetDevicesArgs { Page = 1, PageSize = 2, DeviceTypeFilter = deviceType.ToString() };

        // Act
        PagedData<Device> result = _deviceRepository.GetPaged(args);

        // Assert
        result.Data.Should().HaveCount(1);
        result.Data.Exists(d => d.Type == deviceType).Should().BeTrue();
    }

    [TestMethod]
    public void GetDevices_WhenFilteredByDeviceType_ReturnsFilteredDevices()
    {
        // Arrange
        var deviceTypeFilter = "Sensor";
        var args = new GetDevicesArgs { Page = 1, PageSize = 2, DeviceTypeFilter = deviceTypeFilter };

        // Act
        PagedData<Device> result = _deviceRepository.GetPaged(args);

        // Assert
        result.Data.Should().HaveCount(1);
        result.Data.Exists(d => d.Id == _secondValidDevice.Id).Should().BeTrue();
    }

    #endregion

    #region Error

    #endregion

    #endregion
}
