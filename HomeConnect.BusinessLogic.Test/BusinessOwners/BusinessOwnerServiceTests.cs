using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Repositories;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Repositories;
using FluentAssertions;
using Moq;

namespace HomeConnect.BusinessLogic.Test.BusinessOwners;

[TestClass]
public class BusinessOwnerServiceTests
{
    private Mock<IUserRepository> _userRepository = null!;
    private Mock<IBusinessRepository> _businessRepository = null!;
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

    private readonly List<string> secondaryPhotos =
        ["https://www.example.com/photo2.jpg", "https://www.example.com/photo3.jpg"];

    private const string Type = "Device Type";

    [TestInitialize]
    public void TestInitialize()
    {
        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
        _businessRepository = new Mock<IBusinessRepository>(MockBehavior.Strict);
        _businessOwnerService =
            new BusinessOwnerService(_userRepository.Object, _businessRepository.Object, _deviceRepository.Object);

        _ownerEmail = "owner@example.com";
        _businessRut = "123456789";
        _businessName = "Test Business";
        _owner = new User("John", "Doe", _ownerEmail, "Password123!",
            new Role());
        _existingBusiness = new Business(_businessRut, "Existing Business", _owner);
    }

    #region CreateBusiness

    #region Success

    [TestMethod]
    public void CreateBusiness_WhenOwnerExists_CreatesBusiness()
    {
        // Arrange
        var args = new CreateBusinessArgs { OwnerId = _ownerEmail, Rut = _businessRut, Name = _businessName };
        _userRepository.Setup(x => x.Exists(_ownerEmail)).Returns(true);
        _userRepository.Setup(x => x.Get(_ownerEmail)).Returns(_owner);
        _businessRepository.Setup(x => x.GetBusinessByOwner(_ownerEmail)).Returns((Business?)null);
        _businessRepository.Setup(x => x.Add(It.IsAny<Business>()));
        _businessRepository.Setup(x => x.GetBusinessByRut(_businessRut)).Returns((Business?)null);

        // Act
        _businessOwnerService.CreateBusiness(args);

        // Assert
        _businessRepository.Verify(x => x.Add(It.Is<Business>(b =>
            b.Rut == _businessRut &&
            b.Name == _businessName &&
            b.Owner.Email == _ownerEmail)));
    }

    [TestMethod]
    public void CreateBusiness_WhenCalledWithValidRequest_ReturnsCorrectRut()
    {
        // Arrange
        var args = new CreateBusinessArgs { OwnerId = _ownerEmail, Rut = _businessRut, Name = _businessName };
        _userRepository.Setup(x => x.Get(_ownerEmail)).Returns(_owner);
        _userRepository.Setup(x => x.Exists(_ownerEmail)).Returns(true);
        _businessRepository.Setup(x => x.GetBusinessByOwner(_ownerEmail)).Returns((Business?)null);
        _businessRepository.Setup(x => x.Add(It.IsAny<Business>()));
        _businessRepository.Setup(x => x.GetBusinessByRut(_businessRut)).Returns((Business?)null);

        // Act
        var returnedRut = _businessOwnerService.CreateBusiness(args);

        // Assert
        _businessRepository.Verify(x => x.Add(It.Is<Business>(b =>
            b.Rut == _businessRut &&
            b.Name == _businessName &&
            b.Owner.Email == _ownerEmail)), Times.Once);
        returnedRut.Should().Be(_businessRut);
    }

    #endregion

    #region Error

    [TestMethod]
    public void CreateBusiness_WhenOwnerAlreadyHasBusiness_ThrowsException()
    {
        // Arrange
        var args = new CreateBusinessArgs { OwnerId = _ownerEmail, Rut = _businessRut, Name = _businessName };
        _userRepository.Setup(x => x.Exists(_ownerEmail)).Returns(true);
        _userRepository.Setup(x => x.Get(_ownerEmail)).Returns(_owner);
        _businessRepository.Setup(x => x.GetBusinessByOwner(_ownerEmail)).Returns(_existingBusiness);

        // Act
        Action act = () => _businessOwnerService.CreateBusiness(args);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Owner already has a business");
        _businessRepository.Verify(x => x.Add(It.IsAny<Business>()), Times.Never);
    }

    [TestMethod]
    public void CreateBusiness_WhenOwnerDoesNotExist_ThrowsException()
    {
        // Arrange
        const string nonexistentEmail = "nonexistent@example.com";
        var args = new CreateBusinessArgs { OwnerId = nonexistentEmail, Rut = _businessRut, Name = _businessName };
        _userRepository.Setup(x => x.Exists(nonexistentEmail)).Returns(false);

        // Act
        Action act = () => _businessOwnerService.CreateBusiness(args);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Owner does not exist");
        _businessRepository.Verify(x => x.Add(It.IsAny<Business>()), Times.Never);
    }

    [TestMethod]
    public void CreateBusiness_WhenBusinessRutAlreadyExists_ThrowsException()
    {
        // Arrange
        var args = new CreateBusinessArgs { OwnerId = _ownerEmail, Rut = _businessRut, Name = _businessName };
        _userRepository.Setup(x => x.Exists(_ownerEmail)).Returns(true);
        _userRepository.Setup(x => x.Get(_ownerEmail)).Returns(_owner);
        _businessRepository.Setup(x => x.GetBusinessByRut(_businessRut)).Returns(_existingBusiness);
        _businessRepository.Setup(x => x.GetBusinessByOwner(_ownerEmail)).Returns((Business?)null);
        _businessRepository.Setup(x => x.Add(It.IsAny<Business>()));
        _businessRepository.Setup(x => x.GetBusinessByRut(_businessRut)).Returns(_existingBusiness);

        // Act
        Action act = () => _businessOwnerService.CreateBusiness(args);

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
        var args = new CreateDeviceArgs
        {
            BusinessRut = "RUTexample",
            Name = DeviceName,
            ModelNumber = ModelNumber,
            Description = Description,
            MainPhoto = MainPhoto,
            SecondaryPhotos = secondaryPhotos,
            Type = Type
        };
        _deviceRepository.Setup(x =>
            x.EnsureDeviceDoesNotExist(It.IsAny<Device>()));
        _deviceRepository.Setup(x => x.Add(It.IsAny<Device>()));
        _businessRepository.Setup(x => x.GetBusinessByRut("RUTexample"))
            .Returns(new Business("RUTexample", "Business Name", _owner));

        // Act
        _businessOwnerService.CreateDevice(args);

        // Assert
        _deviceRepository.Verify(x => x.Add(It.Is<Device>(d =>
            d.Name == DeviceName &&
            d.ModelNumber == ModelNumber &&
            d.Description == Description &&
            d.MainPhoto == MainPhoto &&
            d.SecondaryPhotos.SequenceEqual(secondaryPhotos) &&
            d.Type == Type)));
    }

    [TestMethod]
    public void CreateDevice_ReturnsCorrectId()
    {
        // Arrange
        var args = new CreateDeviceArgs
        {
            BusinessRut = "RUTexample",
            Name = DeviceName,
            ModelNumber = ModelNumber,
            Description = Description,
            MainPhoto = MainPhoto,
            SecondaryPhotos = secondaryPhotos,
            Type = Type
        };
        Device addedDevice = null;
        _deviceRepository.Setup(x =>
            x.EnsureDeviceDoesNotExist(It.IsAny<Device>()));
        _deviceRepository.Setup(x => x.Add(It.IsAny<Device>()))
            .Callback<Device>(d => addedDevice = d);
        _businessRepository.Setup(x => x.GetBusinessByRut(args.BusinessRut))
            .Returns(new Business("RUTexample", "Business Name", _owner));

        // Act
        var returnedId = _businessOwnerService.CreateDevice(args);

        // Assert
        Assert.AreEqual(addedDevice.Id, returnedId);
    }

    #endregion

    #region Error

    [TestMethod]
    public void CreateDevice_WhenDeviceAlreadyExists_ThrowsException()
    {
        // Arrange
        var business = new Business("RUTexample", "Business Name", _owner);
        var existingDevice = new Device(DeviceName, ModelNumber, Description,
            MainPhoto, secondaryPhotos, Type, business);
        _deviceRepository
            .Setup(x => x.EnsureDeviceDoesNotExist(It.IsAny<Device>()))
            .Throws(new ArgumentException("Device already exists"));
        _deviceRepository.Setup(x => x.Add(It.IsAny<Device>()));
        _businessRepository.Setup(x => x.GetBusinessByRut("RUTexample")).Returns(business);
        var args = new CreateDeviceArgs
        {
            BusinessRut = "RUTexample",
            Name = DeviceName,
            ModelNumber = ModelNumber,
            Description = Description,
            MainPhoto = MainPhoto,
            SecondaryPhotos = secondaryPhotos,
            Type = Type
        };

        // Act
        Action act = () =>
            _businessOwnerService.CreateDevice(args);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Device already exists");
        _deviceRepository.Verify(x => x.Add(It.IsAny<Device>()), Times.Never);
    }

    #endregion

    #endregion

    #region CreateCamera

    #region Success

    [TestMethod]
    public void CreateCamera_WhenCameraDoesNotExist_CreatesCamera()
    {
        // Arrange
        var business = new Business("RUTexample", "Business Name", _owner);
        _deviceRepository.Setup(x =>
            x.EnsureDeviceDoesNotExist(It.IsAny<Device>()));
        _deviceRepository.Setup(x => x.Add(It.IsAny<Device>()));
        _businessRepository.Setup(x => x.GetBusinessByRut("RUTexample")).Returns(business);
        var args = new CreateCameraArgs
        {
            BusinessRut = "RUTexample",
            Name = DeviceName,
            ModelNumber = ModelNumber,
            Description = Description,
            MainPhoto = MainPhoto,
            SecondaryPhotos = secondaryPhotos,
            MotionDetection = false,
            PersonDetection = false,
            IsExterior = false,
            IsInterior = true
        };

        // Act
        _businessOwnerService.CreateCamera(args);

        // Assert
        _deviceRepository.Verify(x => x.Add(It.Is<Camera>(d =>
            d.Name == DeviceName &&
            d.ModelNumber == ModelNumber &&
            d.Description == Description &&
            d.MainPhoto == MainPhoto &&
            d.SecondaryPhotos.SequenceEqual(secondaryPhotos) &&
            d.Business == business &&
            d.MotionDetection == false &&
            d.PersonDetection == false &&
            d.IsExterior == false &&
            d.IsInterior == true)));
    }

    [TestMethod]
    public void CreateCamera_ReturnsCorrectId()
    {
        // Arrange
        var business = new Business("RUTexample", "Business Name", _owner);
        var args = new CreateCameraArgs
        {
            BusinessRut = "RUTexample",
            Name = DeviceName,
            ModelNumber = ModelNumber,
            Description = Description,
            MainPhoto = MainPhoto,
            SecondaryPhotos = secondaryPhotos,
            MotionDetection = false,
            PersonDetection = false,
            IsExterior = false,
            IsInterior = true
        };
        Camera addedCamera = null;
        _deviceRepository.Setup(x =>
            x.EnsureDeviceDoesNotExist(It.IsAny<Device>()));
        _deviceRepository.Setup(x => x.Add(It.IsAny<Device>()))
            .Callback<Device>(d => addedCamera = (Camera)d);
        _businessRepository.Setup(x => x.GetBusinessByRut(args.BusinessRut)).Returns(business);

        // Act
        var returnedId = _businessOwnerService.CreateCamera(args);

        // Assert
        Assert.AreEqual(addedCamera.Id, returnedId);
    }

    #endregion

    #region Error

    [TestMethod]
    public void CreateCamera_WhenCameraAlreadyExists_ThrowsException()
    {
        // Arrange
        var business = new Business("RUTexample", "Business Name", _owner);
        var existingCamera = new Camera(DeviceName, ModelNumber, Description, MainPhoto, secondaryPhotos, business,
            false, false, false, true);
        _deviceRepository
            .Setup(x => x.EnsureDeviceDoesNotExist(It.IsAny<Device>()))
            .Throws(new ArgumentException("Device already exists"));
        _deviceRepository.Setup(x => x.Add(It.IsAny<Device>()));
        _businessRepository.Setup(x => x.GetBusinessByRut("RUTexample")).Returns(business);
        var args = new CreateCameraArgs
        {
            BusinessRut = "RUTexample",
            Name = DeviceName,
            ModelNumber = ModelNumber,
            Description = Description,
            MainPhoto = MainPhoto,
            SecondaryPhotos = secondaryPhotos,
            MotionDetection = false,
            PersonDetection = false,
            IsExterior = false,
            IsInterior = true
        };

        // Act
        Action act = () => _businessOwnerService.CreateCamera(args);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Device already exists");
        _deviceRepository.Verify(x => x.Add(It.IsAny<Device>()), Times.Never);
    }

    #endregion

    #endregion
}
