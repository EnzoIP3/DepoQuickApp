using BusinessLogic;
using FluentAssertions;
using Moq;

namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public class BusinessOwnerServiceTests
{
    private Mock<IUserRepository> _userRepository = null!;
    private Mock<IBusinessRepository> _businessRepository = null!;
    private Mock<IRoleRepository> _roleRepository = null!;
    private Mock<IDeviceRepository> _deviceRepository = null!;
    private BusinessOwnerService _businessOwnerService = null!;
    private string _ownerEmail = null!;
    private string _businessRut = null!;
    private string _businessName = null!;
    private User _owner = null!;
    private Business _existingBusiness = null!;

    private const string DeviceName = "Device Name";
    private const int ModelNumber = 123;
    private const string Description = "Device Description";
    private const string MainPhoto = "https://www.example.com/photo1.jpg";
    private readonly List<string> SecondaryPhotos = new List<string> { "https://www.example.com/photo2.jpg", "https://www.example.com/photo3.jpg" };
    private const string Type = "Device Type";

    [TestInitialize]
    public void TestInitialize()
    {
        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
        _businessRepository = new Mock<IBusinessRepository>(MockBehavior.Strict);
        _roleRepository = new Mock<IRoleRepository>(MockBehavior.Strict);
        _businessOwnerService = new BusinessOwnerService(_userRepository.Object, _businessRepository.Object, _roleRepository.Object, _deviceRepository.Object);

        _ownerEmail = "owner@example.com";
        _businessRut = "123456789";
        _businessName = "Test Business";
        _owner = new User("John", "Doe", _ownerEmail, "Password123!", new Role());
        _existingBusiness = new Business(_businessRut, "Existing Business", _owner);
    }

    #region CreateBusiness

    #region Success

    [TestMethod]
    public void CreateBusiness_WhenOwnerExists_CreatesBusiness()
    {
        // Arrange
        _userRepository.Setup(x => x.GetUser(_ownerEmail)).Returns(_owner);
        _businessRepository.Setup(x => x.GetBusinessByOwner(_ownerEmail)).Returns((Business?)null);
        _businessRepository.Setup(x => x.Add(It.IsAny<Business>()));
        _businessRepository.Setup(x => x.GetBusinessByRut(_businessRut)).Returns((Business?)null);

        // Act
        _businessOwnerService.CreateBusiness(_ownerEmail, _businessRut, _businessName);

        // Assert
        _businessRepository.Verify(x => x.Add(It.Is<Business>(b =>
            b.Rut == _businessRut &&
            b.Name == _businessName &&
            b.Owner.Email == _ownerEmail)));
    }

    #endregion

    #region Failure

    [TestMethod]
    public void CreateBusiness_WhenOwnerAlreadyHasBusiness_ThrowsException()
    {
        // Arrange
        _userRepository.Setup(x => x.GetUser(_ownerEmail)).Returns(_owner);
        _businessRepository.Setup(x => x.GetBusinessByOwner(_ownerEmail)).Returns(_existingBusiness);

        // Act
        Action act = () => _businessOwnerService.CreateBusiness(_ownerEmail, _businessRut, _businessName);

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Owner already has a business");
        _businessRepository.Verify(x => x.Add(It.IsAny<Business>()), Times.Never);
    }

    [TestMethod]
    public void CreateBusiness_WhenOwnerDoesNotExist_ThrowsException()
    {
        // Arrange
        var nonexistentEmail = "nonexistent@example.com";
        _userRepository.Setup(x => x.GetUser(nonexistentEmail)).Returns((User?)null);

        // Act
        Action act = () => _businessOwnerService.CreateBusiness(nonexistentEmail, _businessRut, _businessName);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Owner does not exist");
        _businessRepository.Verify(x => x.Add(It.IsAny<Business>()), Times.Never);
    }

    [TestMethod]
    public void CreateBusiness_WhenBusinessRutAlreadyExists_ThrowsException()
    {
        // Arrange
        _userRepository.Setup(x => x.GetUser(_ownerEmail)).Returns(_owner);
        _businessRepository.Setup(x => x.GetBusinessByRut(_businessRut)).Returns(_existingBusiness);
        _businessRepository.Setup(x => x.GetBusinessByOwner(_ownerEmail)).Returns((Business?)null);
        _businessRepository.Setup(x => x.Add(It.IsAny<Business>()));
        _businessRepository.Setup(x => x.GetBusinessByRut(_businessRut)).Returns(_existingBusiness);

        // Act
        Action act = () => _businessOwnerService.CreateBusiness(_ownerEmail, _businessRut, _businessName);

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("RUT already exists");
        _businessRepository.Verify(x => x.Add(It.IsAny<Business>()), Times.Never);
    }

    #endregion

    #endregion

    #region CreateDevice

    #region Success
    [TestMethod]
    public void CreateDevice_WhenDeviceDoesNotExist_CreatesDevice()
    {
        // Arrange
        var business = new Business("RUTexample", "Business Name", _owner);
        _deviceRepository.Setup(x => x.EnsureDeviceDoesNotExist(It.IsAny<Device>()));
        _deviceRepository.Setup(x => x.Add(It.IsAny<Device>()));

        // Act
        _businessOwnerService.CreateDevice(DeviceName, ModelNumber, Description, MainPhoto, SecondaryPhotos, Type, business);

        // Assert
        _deviceRepository.Verify(x => x.Add(It.Is<Device>(d =>
            d.Name == DeviceName &&
            d.ModelNumber == ModelNumber &&
            d.Description == Description &&
            d.MainPhoto == MainPhoto &&
            d.SecondaryPhotos.SequenceEqual(SecondaryPhotos) &&
            d.Type == Type)));
    }

    #endregion

    #region Error

    [TestMethod]
    public void CreateDevice_WhenDeviceAlreadyExists_ThrowsException()
    {
        // Arrange
        var business = new Business("RUTexample", "Business Name", _owner);
        var existingDevice = new Device(DeviceName, ModelNumber, Description, MainPhoto, SecondaryPhotos, Type, business);
        _deviceRepository.Setup(x => x.EnsureDeviceDoesNotExist(It.IsAny<Device>())).Throws(new ArgumentException("Device already exists"));
        _deviceRepository.Setup(x => x.Add(It.IsAny<Device>()));

        // Act
        Action act = () => _businessOwnerService.CreateDevice(DeviceName, ModelNumber, Description, MainPhoto, SecondaryPhotos, Type, business);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Device already exists");
        _deviceRepository.Verify(x => x.Add(It.IsAny<Device>()), Times.Never);
    }

    #endregion
    #endregion
}
