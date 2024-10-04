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
            [], "Sensor", new Business("123456", "BusinessValid", _validUser));
        _context.Add(_validDevice);
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
}
