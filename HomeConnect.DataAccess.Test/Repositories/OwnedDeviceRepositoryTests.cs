using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.DataAccess.Repositories;

namespace HomeConnect.DataAccess.Test.Repositories;

[TestClass]
public class OwnedDeviceRepositoryTests
{
    private readonly Context _context = DbContextBuilder.BuildTestDbContext();
    private Business _business = null!;
    private User _businessOwner = null!;
    private Device _device = null!;
    private Home _home = null!;
    private User _homeOwner = null!;
    private OwnedDevice _ownedDevice = null!;
    private OwnedDeviceRepository _ownedDeviceRepository = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();

        Role homeOwnerRole = _context.Roles.First(r => r.Name == "HomeOwner");
        Role businessOwnerRole = _context.Roles.First(r => r.Name == "BusinessOwner");

        _homeOwner = new User("John", "Doe", "email@email.com", "Password#100", homeOwnerRole);
        _businessOwner = new User("Jane", "Doe", "email2@email.com", "Password#100", businessOwnerRole);

        _home = new Home(_homeOwner, "Main St 123", 12.5, 12.5, 5);
        _business = new Business("123456789123", "Business Name", "https://example.com/image.png", _businessOwner);
        _device = new Device("Sensor", 12345, "A sensor", "https://sensor.com/image.png", [], "Sensor", _business);

        _ownedDevice = new OwnedDevice(_home, _device) { Connected = false };

        _context.Users.Add(_homeOwner);
        _context.Users.Add(_businessOwner);
        _context.Homes.Add(_home);
        _context.Businesses.Add(_business);
        _context.Devices.Add(_device);
        _context.OwnedDevices.Add(_ownedDevice);

        _context.SaveChanges();

        _ownedDeviceRepository = new OwnedDeviceRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    #region Add

    [TestMethod]
    public void Add_WhenOwnedDeviceDoesNotExist_AddsOwnedDevice()
    {
        // Act
        var newOwnedDevice = new OwnedDevice(_home, _device);
        _ownedDeviceRepository.Add(newOwnedDevice);

        // Assert
        _context.OwnedDevices.Should().Contain(newOwnedDevice);
    }

    #endregion

    #region GetOwnedDevicesByHome

    [TestMethod]
    public void GetOwnedDevicesByHome_WhenHomeExists_ReturnsOwnedDevices()
    {
        // Act
        IEnumerable<OwnedDevice> result = _ownedDeviceRepository.GetOwnedDevicesByHome(_home);

        // Assert
        result.Should().Contain(_ownedDevice);
    }

    #endregion

    #region GetByHardwareId

    [TestMethod]
    public void GetByHardwareId_WhenOwnedDeviceExists_ReturnsOwnedDevice()
    {
        // Act
        OwnedDevice result = _ownedDeviceRepository.GetByHardwareId(_ownedDevice.HardwareId);

        // Assert
        result.Should().BeEquivalentTo(_ownedDevice);
    }

    #endregion

    #region Exists

    [TestMethod]
    public void Exists_WhenOwnedDeviceExists_ReturnsTrue()
    {
        // Act
        var result = _ownedDeviceRepository.Exists(_ownedDevice.HardwareId);

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #region Update

    [TestMethod]
    public void Update_WhenOwnedDeviceExists_UpdatesOwnedDevice()
    {
        // Arrange
        _ownedDevice.Connected = true;

        // Act
        _ownedDeviceRepository.Update(_ownedDevice);

        // Assert
        OwnedDevice result = _ownedDeviceRepository.GetByHardwareId(_ownedDevice.HardwareId);
        result.Connected.Should().BeTrue();
    }

    #endregion

    #region UpdateLampState

    #region Error
    [TestMethod]
    public void UpdateLampState_WhenOwnedDeviceIsNotALamp_ThrowsInvalidOperationException()
    {
        // Arrange
        Device device = new Device("Sensor", 12345, "A sensor",
            "https://sensor.com/image.png", [], "Sensor", _business);
        OwnedDevice ownedDevice = new OwnedDevice(_home, device);
        _context.Devices.Add(device);
        _context.OwnedDevices.Add(ownedDevice);
        _context.SaveChanges();

        // Act
        Action act = () => _ownedDeviceRepository.UpdateLampState(ownedDevice.HardwareId, true);

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("The device is not a lamp.");
    }
    #endregion
    #endregion
}
