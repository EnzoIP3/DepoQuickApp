using BusinessLogic;
using FluentAssertions;

namespace HomeConnect.DataAccess.Test;

[TestClass]
public class DeviceRepositoryTests
{
    private readonly Context _context = DbContextBuilder.BuildTestDbContext();
    private DeviceRepository _deviceRepository = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();
        _deviceRepository = new DeviceRepository(_context);
        _context.SaveChanges();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void Add_WhenDeviceDoesNotExist_ShouldAddDevice()
    {
        // Arrange
        var device = new Device("Device", 12345, "Device description", "https://example.com/image.png",
            [], "Sensor");

        // Act
        _deviceRepository.Add(device);

        // Assert
        _context.Devices.Should().Contain(device);
    }

    [TestMethod]
    public void Add_WhenDeviceExists_ShouldThrowException()
    {
        // Arrange
        var device = new Device("Device", 12345, "Device description", "https://example.com/image.png",
            [], "Sensor");
        _deviceRepository.Add(device);

        // Act
        Action act = () => _deviceRepository.Add(device);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
