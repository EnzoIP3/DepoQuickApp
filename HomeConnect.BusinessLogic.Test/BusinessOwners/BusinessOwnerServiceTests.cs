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
    private const string DeviceName = "Device Name";
    private const int ModelNumber = 123;
    private const string Description = "Device Description";
    private const string MainPhoto = "https://www.example.com/photo1.jpg";

    private const string Type = "Device Type";

    private readonly List<string> _secondaryPhotos =
        ["https://www.example.com/photo2.jpg", "https://www.example.com/photo3.jpg"];

    private string _businessLogo = null!;
    private string _businessName = null!;
    private BusinessOwnerService _businessOwnerService = null!;
    private Mock<IBusinessRepository> _businessRepository = null!;
    private string _businessRut = null!;
    private Mock<IDeviceRepository> _deviceRepository = null!;
    private Business _existingBusiness = null!;
    private User _owner = null!;
    private string _ownerEmail = null!;
    private Mock<IUserRepository> _userRepository = null!;

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
        _businessLogo = "https://example.com/image.png";
        _owner = new User("John", "Doe", _ownerEmail, "Password123!", new Role());
        _existingBusiness = new Business(_businessRut, "Existing Business", "https://example.com/image.png", _owner);
    }

    #region CreateBusiness

    #region Success

    [TestMethod]
    public void CreateBusiness_WhenOwnerExists_CreatesBusiness()
    {
        // Arrange
        var args = new CreateBusinessArgs
        {
            OwnerId = _owner.Id.ToString(), Rut = _businessRut, Name = _businessName, Logo = _businessLogo
        };
        _userRepository.Setup(x => x.Exists(_owner.Id)).Returns(true);
        _userRepository.Setup(x => x.Get(_owner.Id)).Returns(_owner);
        _userRepository.Setup(x => x.Exists(_owner.Id)).Returns(true);
        _businessRepository.Setup(x => x.GetByOwnerId(_owner.Id)).Returns(_existingBusiness);
        _businessRepository.Setup(x => x.Add(It.IsAny<Business>()));
        _businessRepository.Setup(x => x.Exists(_businessRut)).Returns(false);
        _businessRepository.Setup(x => x.ExistsByOwnerId(_owner.Id)).Returns(false);

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
        var args = new CreateBusinessArgs
        {
            OwnerId = _owner.Id.ToString(), Rut = _businessRut, Name = _businessName, Logo = _businessLogo
        };
        _userRepository.Setup(x => x.Get(_owner.Id)).Returns(_owner);
        _userRepository.Setup(x => x.Exists(_owner.Id)).Returns(true);
        _businessRepository.Setup(x => x.Add(It.IsAny<Business>()));
        _businessRepository.Setup(x => x.Exists(_businessRut)).Returns(false);
        _businessRepository.Setup(x => x.ExistsByOwnerId(_owner.Id)).Returns(false);

        // Act
        Business returnedBusiness = _businessOwnerService.CreateBusiness(args);

        // Assert
        _businessRepository.Verify(x => x.Add(It.Is<Business>(b =>
            b.Rut == _businessRut &&
            b.Name == _businessName &&
            b.Owner.Email == _ownerEmail)), Times.Once);
        returnedBusiness.Rut.Should().Be(_businessRut);
    }

    #endregion

    #region Error

    [TestMethod]
    public void CreateBusiness_WhenOwnerAlreadyHasBusiness_ThrowsException()
    {
        // Arrange
        var args = new CreateBusinessArgs { OwnerId = _owner.Id.ToString(), Rut = _businessRut, Name = _businessName };
        _userRepository.Setup(x => x.Exists(_owner.Id)).Returns(true);
        _userRepository.Setup(x => x.Get(_owner.Id)).Returns(_owner);
        _userRepository.Setup(x => x.Exists(_owner.Id)).Returns(true);
        _businessRepository.Setup(x => x.GetByOwnerId(_owner.Id)).Returns(_existingBusiness);
        _businessRepository.Setup(x => x.ExistsByOwnerId(_owner.Id)).Returns(true);

        // Act
        Action act = () => _businessOwnerService.CreateBusiness(args);

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Owner already has a business.");
        _businessRepository.Verify(x => x.Add(It.IsAny<Business>()), Times.Never);
    }

    [TestMethod]
    public void CreateBusiness_WhenOwnerDoesNotExist_ThrowsException()
    {
        // Arrange
        var args = new CreateBusinessArgs { OwnerId = _owner.Id.ToString(), Rut = _businessRut, Name = _businessName };
        _userRepository.Setup(x => x.Exists(_owner.Id)).Returns(false);

        // Act
        Action act = () => _businessOwnerService.CreateBusiness(args);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("That business owner does not exist.");
        _businessRepository.Verify(x => x.Add(It.IsAny<Business>()), Times.Never);
    }

    [TestMethod]
    public void CreateBusiness_WhenBusinessRutAlreadyExists_ThrowsException()
    {
        // Arrange
        var args = new CreateBusinessArgs { OwnerId = _owner.Id.ToString(), Rut = _businessRut, Name = _businessName };
        _userRepository.Setup(x => x.Exists(_owner.Id)).Returns(true);
        _userRepository.Setup(x => x.Get(_owner.Id)).Returns(_owner);
        _businessRepository.Setup(x => x.Exists(_businessRut)).Returns(true);
        _businessRepository.Setup(x => x.ExistsByOwnerId(_owner.Id)).Returns(false);
        _businessRepository.Setup(x => x.Get(_businessRut)).Returns(_existingBusiness);

        // Act
        Action act = () => _businessOwnerService.CreateBusiness(args);

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("There is already a business with this RUT.");
        _businessRepository.Verify(x => x.Add(It.IsAny<Business>()), Times.Never);
    }

    [TestMethod]
    public void CreateBusiness_WhenOwnerIdIsNotGuid_ThrowsException()
    {
        // Arrange
        var args = new CreateBusinessArgs { OwnerId = "not-a-guid", Rut = _businessRut, Name = _businessName };

        // Act
        Action act = () => _businessOwnerService.CreateBusiness(args);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("The business owner ID is not a valid GUID.");
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
            Owner = _owner, // Pass the owner instead of BusinessRut
            Name = DeviceName,
            ModelNumber = ModelNumber,
            Description = Description,
            MainPhoto = MainPhoto,
            SecondaryPhotos = _secondaryPhotos,
            Type = DeviceType.Camera.ToString() // Use a valid DeviceType enum value
        };

        _deviceRepository.Setup(x => x.ExistsByModelNumber(It.IsAny<Guid>()));
        _deviceRepository.Setup(x => x.Add(It.IsAny<Device>()));
        _businessRepository.Setup(x => x.GetByOwnerId(_owner.Id))
            .Returns(new Business("123456789", "Business Name", "https://example.com/image.png", _owner));
        _businessRepository.Setup(x => x.ExistsByOwnerId(_owner.Id)).Returns(true);

        // Act
        _businessOwnerService.CreateDevice(args);

        // Assert
        _deviceRepository.Verify(x => x.Add(It.Is<Device>(d =>
            d.Name == DeviceName &&
            d.ModelNumber == ModelNumber &&
            d.Description == Description &&
            d.MainPhoto == MainPhoto &&
            d.SecondaryPhotos.SequenceEqual(_secondaryPhotos) &&
            d.Type.ToString() == DeviceType.Camera.ToString()))); // Validate Device creation
    }

    #endregion

    #region Error

    [TestMethod]
    public void CreateDevice_WhenDeviceAlreadyExists_ThrowsException()
    {
        // Arrange
        var business = new Business("12345", "Business Name", "https://example.com/image.png", _owner);
        _deviceRepository
            .Setup(x => x.ExistsByModelNumber(It.IsAny<Guid>()))
            .Returns(false);
        _deviceRepository.Setup(x => x.Add(It.IsAny<Device>()));
        _businessRepository.Setup(x => x.GetByOwnerId(_owner.Id)).Returns(business);
        _businessRepository.Setup(x => x.ExistsByOwnerId(_owner.Id)).Returns(true);
        var args = new CreateDeviceArgs
        {
            Owner = _owner,
            Name = DeviceName,
            ModelNumber = ModelNumber,
            Description = Description,
            MainPhoto = MainPhoto,
            SecondaryPhotos = _secondaryPhotos,
            Type = DeviceType.Camera.ToString()
        };

        // Act
        Action act = () =>
            _businessOwnerService.CreateDevice(args);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Device already exists");
        _deviceRepository.Verify(x => x.Add(It.IsAny<Device>()), Times.Never);
    }

    [TestMethod]
    public void CreateDevice_WhenBusinessDoesNotExist_ThrowsException()
    {
        // Arrange
        var args = new CreateDeviceArgs
        {
            Owner = _owner,
            Name = DeviceName,
            ModelNumber = ModelNumber,
            Description = Description,
            MainPhoto = MainPhoto,
            SecondaryPhotos = _secondaryPhotos,
            Type = Type
        };
        _deviceRepository.Setup(x =>
            x.ExistsByModelNumber(It.IsAny<Guid>())).Returns(true);
        _deviceRepository.Setup(x => x.Add(It.IsAny<Device>()));
        _businessRepository.Setup(x => x.ExistsByOwnerId(args.Owner.Id)).Returns(false);

        // Act
        Action act = () => _businessOwnerService.CreateDevice(args);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("That business does not exist.");
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
        var business = new Business("RUTexample", "Business Name", "https://example.com/image.png", _owner);
        _deviceRepository.Setup(x =>
            x.ExistsByModelNumber(It.IsAny<Guid>())).Returns(false);
        _deviceRepository.Setup(x => x.Add(It.IsAny<Device>()));
        _businessRepository.Setup(x => x.GetByOwnerId(_owner.Id)).Returns(business);
        _businessRepository.Setup(x => x.ExistsByOwnerId(_owner.Id)).Returns(true);
        var args = new CreateCameraArgs
        {
            Owner = _owner,
            Name = DeviceName,
            ModelNumber = ModelNumber,
            Description = Description,
            MainPhoto = MainPhoto,
            SecondaryPhotos = _secondaryPhotos,
            MotionDetection = false,
            PersonDetection = false,
            Exterior = false,
            Interior = true
        };

        // Act
        _businessOwnerService.CreateCamera(args);

        // Assert
        _deviceRepository.Verify(x => x.Add(It.Is<Camera>(d =>
            d.Name == DeviceName &&
            d.ModelNumber == ModelNumber &&
            d.Description == Description &&
            d.MainPhoto == MainPhoto &&
            d.SecondaryPhotos.SequenceEqual(_secondaryPhotos) &&
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
        var business = new Business("RUTexample", "Business Name", "https://example.com/image.png", _owner);
        var args = new CreateCameraArgs
        {
            Owner = _owner,
            Name = DeviceName,
            ModelNumber = ModelNumber,
            Description = Description,
            MainPhoto = MainPhoto,
            SecondaryPhotos = _secondaryPhotos,
            MotionDetection = false,
            PersonDetection = false,
            Exterior = false,
            Interior = true
        };
        var addedCamera = new Camera();
        _deviceRepository.Setup(x =>
            x.ExistsByModelNumber(It.IsAny<Guid>())).Returns(false);
        _deviceRepository.Setup(x => x.Add(It.IsAny<Device>()))
            .Callback<Device>(d => addedCamera = (Camera)d);
        _businessRepository.Setup(x => x.GetByOwnerId(_owner.Id)).Returns(business);
        _businessRepository.Setup(x => x.ExistsByOwnerId(_owner.Id)).Returns(true);

        // Act
        Camera returnedCamera = _businessOwnerService.CreateCamera(args);

        // Assert
        Assert.AreEqual(addedCamera.Id, returnedCamera.Id);
    }

    #endregion

    #region Error

    [TestMethod]
    public void CreateCamera_WhenCameraAlreadyExists_ThrowsException()
    {
        // Arrange
        var business = new Business("RUTexample", "Business Name", "https://example.com/image.png", _owner);
        _deviceRepository
            .Setup(x => x.ExistsByModelNumber(It.IsAny<Guid>()))
            .Returns(true);
        _deviceRepository.Setup(x => x.Add(It.IsAny<Device>()));
        _businessRepository.Setup(x => x.GetByOwnerId(_owner.Id)).Returns(business);
        _businessRepository.Setup(x => x.ExistsByOwnerId(_owner.Id)).Returns(true);
        var args = new CreateCameraArgs
        {
            Owner = _owner,
            Name = DeviceName,
            ModelNumber = ModelNumber,
            Description = Description,
            MainPhoto = MainPhoto,
            SecondaryPhotos = _secondaryPhotos,
            MotionDetection = false,
            PersonDetection = false,
            Exterior = false,
            Interior = true
        };

        // Act
        Action act = () => _businessOwnerService.CreateCamera(args);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Device already exists");
        _deviceRepository.Verify(x => x.Add(It.IsAny<Device>()), Times.Never);
    }

    [TestMethod]
    public void CreateCamera_WhenBusinessDoesNotExist_ThrowsException()
    {
        // Arrange
        var args = new CreateCameraArgs
        {
            Owner = _owner,
            Name = DeviceName,
            ModelNumber = ModelNumber,
            Description = Description,
            MainPhoto = MainPhoto,
            SecondaryPhotos = _secondaryPhotos,
            MotionDetection = false,
            PersonDetection = false,
            Exterior = false,
            Interior = true
        };

        _deviceRepository.Setup(x =>
            x.ExistsByModelNumber(It.IsAny<Guid>())).Returns(true);
        _deviceRepository.Setup(x => x.Add(It.IsAny<Device>()));
        _businessRepository.Setup(x => x.ExistsByOwnerId(_owner.Id)).Returns(false);

        // Act
        Action act = () => _businessOwnerService.CreateCamera(args);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("That business does not exist.");
        _deviceRepository.Verify(x => x.Add(It.IsAny<Device>()), Times.Never);
    }

    #endregion

    #endregion
}
