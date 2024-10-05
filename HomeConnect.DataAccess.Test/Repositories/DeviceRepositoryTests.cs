using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
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
    private Device _validDevice = null!;
    private Device _secondValidDevice = null!;
    private User _validUser = null!;
    private Role _role = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();
        _deviceRepository = new DeviceRepository(_context);
        _role = new Role();
        _validUser = new User("John", "Doe", "johhnDoe@example.com", "Password#100", _role);
        _validDevice = new Device("DeviceValid", 123456, "Device description", "https://example.com/image.png",
            [], "Camera", new Business("123456", "BusinessValid", _validUser));
        _secondValidDevice = new Device("DeviceValid2", 1234567, "Device description", "https://example2.com/image.png",
            [], "Sensor", new Business("1234567", "BusinessValid2", _validUser));
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
    public void Add_WhenDeviceDoesNotExist_ShouldAddDevice()
    {
        // Arrange
        var business = new Business("12345", "Business", _validUser);
        var device = new Device("Device", 12345, "Device description", "https://example.com/image.png",
            [], "Sensor", business);

        // Act
        _deviceRepository.Add(device);

        // Assert
        _context.Devices.Should().Contain(device);
    }

    #endregion

    #region Error

    [TestMethod]
    public void Add_WhenDeviceExists_ShouldThrowException()
    {
        // Arrange
        var business = new Business("12345", "Business", _validUser);
        var device = new Device("Device", 12345, "Device description", "https://example.com/image.png",
            [], "Sensor", business);
        _deviceRepository.Add(device);

        // Act
        Action act = () => _deviceRepository.Add(device);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #endregion

    #region Get

    #region Success

    [TestMethod]
    public void Get_WhenDeviceExists_ShouldReturnDevice()
    {
        // Arrange
        var business = new Business("12345", "Business", _validUser);
        var device = new Device("Device", 12345, "Device description", "https://example.com/image.png",
            [], "Sensor", business);
        _deviceRepository.Add(device);
        _context.SaveChanges();

        // Act
        var result = _deviceRepository.Get(device.Id);

        // Assert
        result.Should().BeEquivalentTo(device);
    }

    #endregion

    #region Error

    [TestMethod]
    public void Get_WhenDeviceDoesNotExist_ShouldThrowArgumentException()
    {
        // Arrange
        var nonExistentDeviceId = Guid.NewGuid();

        // Act
        Action act = () => _deviceRepository.Get(nonExistentDeviceId);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #endregion

    #region GetDevices

    #region Success
    [TestMethod]
    public void GetDevices_WhenCalled_ReturnsPaginatedDevices()
    {
        // Act
        var result = _deviceRepository.GetDevices(1, 2, null, null, null, null);

        // Assert
        result.Data.Should().HaveCount(2);
        result.Data.Exists(d => d.Id == _validDevice.Id).Should().BeTrue();
        result.Data.Exists(d => d.Id == _secondValidDevice.Id).Should().BeTrue();
    }

    [TestMethod]
    public void GetDevices_WhenFilteredByDeviceName_ReturnsFilteredDevices()
    {
        // Act
        var result = _deviceRepository.GetDevices(1, 10, "DeviceValid");

        // Assert
        result.Data.Should().HaveCount(1);
        result.Data.First().Name.Should().Be("DeviceValid");
    }

    [TestMethod]
    public void GetDevices_WhenFilteredByModelNumber_ReturnsFilteredDevices()
    {
        // Act
        var result = _deviceRepository.GetDevices(1, 10, null, 1234567);

        // Assert
        result.Data.Should().HaveCount(1);
        result.Data.First().ModelNumber.Should().Be(1234567);
    }

    [TestMethod]
    public void GetDevices_WhenFilteredByBusinessName_ReturnsFilteredDevices()
    {
        // Arrange
        var businessNameFilter = "BusinessValid2";

        // Act
        var result = _deviceRepository.GetDevices(1, 10, null, null, businessNameFilter);

        // Assert
        result.Data.Should().HaveCount(1);
        result.Data[0].Business.Name.Should().Be(businessNameFilter);
    }

    [TestMethod]
    public void GetDevices_WhenFilteredByDeviceType_ReturnsFilteredDevices()
    {
        // Arrange
        var deviceTypeFilter = "Sensor";

        // Act
        var result = _deviceRepository.GetDevices(1, 2, null, null, null, deviceTypeFilter);

        // Assert
        result.Data.Should().HaveCount(1);
        result.Data.Exists(d => d.Id == _secondValidDevice.Id).Should().BeTrue();
    }

    #endregion

    #region Error

    #endregion

    #endregion
}
