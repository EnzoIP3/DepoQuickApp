using BusinessLogic;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Repositories;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Repositories;
using FluentAssertions;
using ModeloValidador.Abstracciones;
using Moq;

namespace HomeConnect.BusinessLogic.Test.BusinessOwners;

[TestClass]
public class BusinessOwnerServiceTests
{
    private const string DeviceName = "Device Name";
    private const string ModelNumber = "123";
    private const string Description = "Device Description";
    private const string MainPhoto = "https://www.example.com/photo1.jpg";

    private const string Type = "Device Type";

    private readonly List<string> _secondaryPhotos =
        ["https://www.example.com/photo2.jpg", "https://www.example.com/photo3.jpg"];

    private readonly Guid _validatorId = Guid.NewGuid();

    private string _businessLogo = null!;
    private string _businessName = null!;
    private BusinessOwnerService _businessOwnerService = null!;
    private Mock<IBusinessRepository> _businessRepository = null!;
    private string _businessRut = null!;
    private Mock<IDeviceRepository> _deviceRepository = null!;
    private Business _existingBusiness = null!;
    private Mock<IModeloValidador> _modeloValidador = null!;
    private User _owner = null!;
    private string _ownerEmail = null!;
    private Mock<IUserRepository> _userRepository = null!;
    private Mock<IValidatorService> _validatorService = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _deviceRepository = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
        _businessRepository = new Mock<IBusinessRepository>(MockBehavior.Strict);
        _validatorService = new Mock<IValidatorService>(MockBehavior.Strict);
        _modeloValidador = new Mock<IModeloValidador>(MockBehavior.Strict);
        _businessOwnerService =
            new BusinessOwnerService(_userRepository.Object, _businessRepository.Object, _deviceRepository.Object,
                _validatorService.Object);

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
            OwnerId = _owner.Id.ToString(),
            Rut = _businessRut,
            Name = _businessName,
            Logo = _businessLogo
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
            OwnerId = _owner.Id.ToString(),
            Rut = _businessRut,
            Name = _businessName,
            Logo = _businessLogo
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

    [TestMethod]
    public void CreateBusiness_WhenCalledWithAValidValidator_CreatesBusiness()
    {
        // Arrange
        var args = new CreateBusinessArgs
        {
            OwnerId = _owner.Id.ToString(),
            Rut = _businessRut,
            Name = _businessName,
            Logo = _businessLogo,
            Validator = "validator"
        };
        var validatorId = Guid.NewGuid();
        _userRepository.Setup(x => x.Exists(_owner.Id)).Returns(true);
        _userRepository.Setup(x => x.Get(_owner.Id)).Returns(_owner);
        _userRepository.Setup(x => x.Exists(_owner.Id)).Returns(true);
        _businessRepository.Setup(x => x.GetByOwnerId(_owner.Id)).Returns(_existingBusiness);
        _businessRepository.Setup(x => x.Add(It.IsAny<Business>()));
        _businessRepository.Setup(x => x.Exists(_businessRut)).Returns(false);
        _businessRepository.Setup(x => x.ExistsByOwnerId(_owner.Id)).Returns(false);
        _validatorService.Setup(x => x.Exists(args.Validator)).Returns(true);
        _validatorService.Setup(x => x.GetValidatorIdByName(args.Validator)).Returns(validatorId);

        // Act
        _businessOwnerService.CreateBusiness(args);

        // Assert
        _businessRepository.Verify(x => x.Add(It.Is<Business>(b =>
            b.Rut == _businessRut &&
            b.Name == _businessName &&
            b.Owner.Email == _ownerEmail &&
            b.Validator == validatorId)));
        _validatorService.Verify(x => x.GetValidatorIdByName(args.Validator), Times.Once);
        _validatorService.Verify(x => x.Exists(args.Validator), Times.Once);
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

    [TestMethod]
    public void CreateBusiness_WhenValidatorDoesNotExists_ThrowsException()
    {
        // Arrange
        var args = new CreateBusinessArgs
        {
            OwnerId = _owner.Id.ToString(),
            Rut = _businessRut,
            Name = _businessName,
            Logo = _businessLogo,
            Validator = "validator"
        };
        _userRepository.Setup(x => x.Exists(_owner.Id)).Returns(true);
        _userRepository.Setup(x => x.Get(_owner.Id)).Returns(_owner);
        _userRepository.Setup(x => x.Exists(_owner.Id)).Returns(true);
        _businessRepository.Setup(x => x.GetByOwnerId(_owner.Id)).Returns(_existingBusiness);
        _businessRepository.Setup(x => x.Add(It.IsAny<Business>()));
        _businessRepository.Setup(x => x.Exists(_businessRut)).Returns(false);
        _businessRepository.Setup(x => x.ExistsByOwnerId(_owner.Id)).Returns(false);
        _validatorService.Setup(x => x.Exists(args.Validator)).Returns(false);

        // Act
        Action act = () => _businessOwnerService.CreateBusiness(args);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("The specified validator does not exist.");
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

        _deviceRepository.Setup(x => x.ExistsByModelNumber(args.ModelNumber)).Returns(false);
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
            x.ExistsByModelNumber(args.ModelNumber)).Returns(false);
        _deviceRepository.Setup(x => x.Add(It.IsAny<Device>()));
        _businessRepository.Setup(x => x.ExistsByOwnerId(args.Owner.Id)).Returns(false);

        // Act
        Action act = () => _businessOwnerService.CreateDevice(args);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("That business does not exist.");
        _deviceRepository.Verify(x => x.Add(It.IsAny<Device>()), Times.Never);
    }

    [TestMethod]
    public void CreateDevice_WhenHasAValidatorAndModelNumberIsInvalid_ThrowsArgumentException()
    {
        // Arrange
        var business = new Business("12345", "Business Name", "https://example.com/image.png", _owner, _validatorId);
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
            x.ExistsByModelNumber(args.ModelNumber)).Returns(false);
        _deviceRepository.Setup(x => x.Add(It.IsAny<Device>()));
        _businessRepository.Setup(x => x.GetByOwnerId(_owner.Id)).Returns(business);
        _businessRepository.Setup(x => x.ExistsByOwnerId(_owner.Id)).Returns(true);
        _validatorService.Setup(x =>
            x.GetValidator(business.Validator!)).Returns(_modeloValidador.Object);
        _modeloValidador.Setup(x =>
            x.EsValido(It.Is<Modelo>(m => m.Value == args.ModelNumber))).Returns(false);

        // Act
        Action act = () => _businessOwnerService.CreateDevice(args);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The model number is not valid according to the specified validator.");
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
            x.ExistsByModelNumber(args.ModelNumber)).Returns(false);
        _deviceRepository.Setup(x => x.Add(It.IsAny<Device>()));
        _businessRepository.Setup(x => x.GetByOwnerId(_owner.Id)).Returns(business);
        _businessRepository.Setup(x => x.ExistsByOwnerId(_owner.Id)).Returns(true);

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
    public void CreateCamera_WhenCalled_ReturnsCorrectId()
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
            x.ExistsByModelNumber(args.ModelNumber)).Returns(false);
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
            x.ExistsByModelNumber(args.ModelNumber)).Returns(false);
        _deviceRepository.Setup(x => x.Add(It.IsAny<Device>()));
        _businessRepository.Setup(x => x.ExistsByOwnerId(_owner.Id)).Returns(false);

        // Act
        Action act = () => _businessOwnerService.CreateCamera(args);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("That business does not exist.");
        _deviceRepository.Verify(x => x.Add(It.IsAny<Device>()), Times.Never);
    }

    [TestMethod]
    public void CreateCamera_WhenHasAValidatorAndModelNumberIsInvalid_ThrowsArgumentException()
    {
        // Arrange
        var business = new Business("RUTexample", "Business Name", "https://example.com/image.png", _owner,
            _validatorId);
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
            x.ExistsByModelNumber(args.ModelNumber)).Returns(false);
        _deviceRepository.Setup(x => x.Add(It.IsAny<Device>()));
        _businessRepository.Setup(x => x.GetByOwnerId(_owner.Id)).Returns(business);
        _businessRepository.Setup(x => x.ExistsByOwnerId(_owner.Id)).Returns(true);
        _validatorService.Setup(x =>
            x.GetValidator(business.Validator!)).Returns(_modeloValidador.Object);
        _modeloValidador.Setup(x =>
            x.EsValido(It.Is<Modelo>(m => m.Value == args.ModelNumber))).Returns(false);

        // Act
        Action act = () => _businessOwnerService.CreateCamera(args);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The model number is not valid according to the specified validator.");
    }

    #endregion

    #endregion

    #region UpdateValidator

    #region Error

    [TestMethod]
    public void UpdateValidator_WhenBusinessIsNotFromUser_ThrowsInvalidOperationException()
    {
        // Arrange
        var business = new Business("RUTexample", "Business Name", "https://example.com/image.png", _owner);
        var args = new UpdateValidatorArgs
        {
            BusinessRut = business.Rut,
            Validator = "validator",
            OwnerId = Guid.NewGuid().ToString()
        };
        _businessRepository.Setup(x => x.Exists(business.Rut)).Returns(true);
        _businessRepository.Setup(x => x.Get(business.Rut)).Returns(business);

        // Act
        Action act = () => _businessOwnerService.UpdateValidator(args);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("The business does not belong to the specified owner.");
        _businessRepository.Verify(x => x.Get(business.Rut), Times.Once);
    }

    [TestMethod]
    public void UpdateValidator_WhenValidatorDoesNotExist_ThrowsArgumentException()
    {
        // Arrange
        var business = new Business("RUTexample", "Business Name", "https://example.com/image.png", _owner);
        var args = new UpdateValidatorArgs
        {
            BusinessRut = business.Rut,
            Validator = "validator",
            OwnerId = _owner.Id.ToString()
        };
        _businessRepository.Setup(x => x.Exists(business.Rut)).Returns(true);
        _businessRepository.Setup(x => x.Get(business.Rut)).Returns(business);
        _validatorService.Setup(x => x.Exists(args.Validator)).Returns(false);

        // Act
        Action act = () => _businessOwnerService.UpdateValidator(args);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("The specified validator does not exist.");
        _businessRepository.Verify(x => x.Get(business.Rut), Times.Once);
        _businessRepository.Verify(x => x.Exists(business.Rut), Times.Once);
        _validatorService.Verify(x => x.Exists(args.Validator), Times.Once);
    }

    [TestMethod]
    public void UpdateValidator_WhenBusinessDoesNotExist_ThrowsArgumentException()
    {
        // Arrange
        var args = new UpdateValidatorArgs
        {
            BusinessRut = "RUTexample",
            Validator = "validator",
            OwnerId = _owner.Id.ToString()
        };
        _businessRepository.Setup(x => x.Exists(args.BusinessRut)).Returns(false);

        // Act
        Action act = () => _businessOwnerService.UpdateValidator(args);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("The business does not exist.");
        _businessRepository.Verify(x => x.Exists(args.BusinessRut), Times.Once);
    }

    #endregion

    #region Success

    [TestMethod]
    public void UpdateValidator_WhenCalledWithValidRequest_UpdatesValidator()
    {
        // Arrange
        var business = new Business("RUTexample", "Business Name", "https://example.com/image.png", _owner);
        var args = new UpdateValidatorArgs
        {
            BusinessRut = business.Rut,
            Validator = "validator",
            OwnerId = _owner.Id.ToString()
        };
        var validatorId = Guid.NewGuid();
        _businessRepository.Setup(x => x.Exists(business.Rut)).Returns(true);
        _businessRepository.Setup(x => x.Get(business.Rut)).Returns(business);
        _validatorService.Setup(x => x.Exists(args.Validator)).Returns(true);
        _validatorService.Setup(x => x.GetValidatorIdByName(args.Validator)).Returns(validatorId);
        _businessRepository.Setup(x => x.UpdateValidator(business.Rut, validatorId));

        // Act
        _businessOwnerService.UpdateValidator(args);

        // Assert
        _businessRepository.Verify(x => x.Exists(business.Rut), Times.Once);
        _businessRepository.Verify(x => x.Get(business.Rut), Times.Once);
        _businessRepository.Verify(x => x.UpdateValidator(business.Rut, validatorId), Times.Once);
        _validatorService.Verify(x => x.GetValidatorIdByName(args.Validator), Times.Once);
        _validatorService.Verify(x => x.Exists(args.Validator), Times.Once);
    }

    [TestMethod]
    public void UpdateValidator_WhenCalledWithValidRequestAndNoValidator_UpdatesValidatorToNull()
    {
        // Arrange
        var business = new Business("RUTexample", "Business Name", "https://example.com/image.png", _owner,
            _validatorId);
        var args = new UpdateValidatorArgs
        {
            BusinessRut = business.Rut,
            Validator = null,
            OwnerId = _owner.Id.ToString()
        };
        _businessRepository.Setup(x => x.Exists(business.Rut)).Returns(true);
        _businessRepository.Setup(x => x.Get(business.Rut)).Returns(business);
        _businessRepository.Setup(x => x.UpdateValidator(business.Rut, null));

        // Act
        _businessOwnerService.UpdateValidator(args);

        // Assert
        _businessRepository.Verify(x => x.Exists(business.Rut), Times.Once);
        _businessRepository.Verify(x => x.Get(business.Rut), Times.Once);
        _businessRepository.Verify(x => x.UpdateValidator(business.Rut, null), Times.Once);
    }

    #endregion

    #endregion

    #region GetBusinesses

    [TestMethod]
    public void GetBusinesses_WhenCalledWithInvalidOwnerId_ThrowsArgumentException()
    {
        // Arrange
        var ownerFilter = "not-a-guid";
        var currentPage = 1;
        var pageSize = 10;

        // Act
        Action act = () => _businessOwnerService.GetBusinesses(ownerFilter, currentPage, pageSize);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("The business owner ID is not a valid GUID.");
    }

    [TestMethod]
    public void GetBusinesses_WhenCalled_ReturnsBusinesses()
    {
        // Arrange
        var businesses = new PagedData<Business>
        {
            Data =
            [
                new Business("RUTexample", "Business Name", "https://example.com/image.png", _owner)
            ],
            Page = 1,
            PageSize = 10,
            TotalPages = 1
        };
        var currentPage = 1;
        var pageSize = 10;
        var filterArgs = new FilterArgs { OwnerIdFilter = _owner.Id, CurrentPage = currentPage, PageSize = pageSize };
        _businessRepository.Setup(x => x.GetPaged(filterArgs)).Returns(businesses);

        // Act
        PagedData<Business> returnedBusinesses =
            _businessOwnerService.GetBusinesses(_owner.Id.ToString(), currentPage, pageSize);

        // Assert
        returnedBusinesses.Should().BeEquivalentTo(businesses);
        _businessRepository.Verify(x => x.GetPaged(
            It.Is<FilterArgs>(a => a.OwnerIdFilter == _owner.Id)), Times.Once);
    }

    #endregion

    #region GetDevices

    #region Error

    [TestMethod]
    public void GetDevices_WhenBusinessDoesNotBelongToUser_ThrowsInvalidOperationException()
    {
        // Arrange
        var business = new Business("RUTexample", "Business Name", "https://example.com/image.png", _owner);
        var user = new User("John", "Doe", "email@email.com", "Password@1", new Role());
        _businessRepository.Setup(x => x.Get(business.Rut)).Returns(business);
        var args = new GetBusinessDevicesArgs { Rut = business.Rut, User = user, CurrentPage = 1, PageSize = 10 };

        // Act
        Action act = () => _businessOwnerService.GetDevices(args);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("The business does not belong to the specified owner.");
        _businessRepository.Verify(x => x.Get(business.Rut), Times.Once);
    }

    #endregion

    #region Success

    [TestMethod]
    public void GetDevices_WhenCalledWithValidRequest_ReturnsDevices()
    {
        // Arrange
        var business = new Business("RUTexample", "Business Name", "https://example.com/image.png", _owner);
        User user = _owner;
        var devices = new PagedData<Device>
        {
            Data =
            [
                new Device("Device Name", "123", "Device Description", "https://www.example.com/photo1.jpg",
                    ["https://www.example.com/photo2.jpg", "https://www.example.com/photo3.jpg"],
                    DeviceType.Sensor.ToString(), business)
            ],
            Page = 1,
            PageSize = 10,
            TotalPages = 1
        };
        _businessRepository.Setup(x => x.Get(business.Rut)).Returns(business);
        _deviceRepository.Setup(x => x.GetPaged(It.IsAny<GetDevicesArgs>())).Returns(devices);
        var args = new GetBusinessDevicesArgs { Rut = business.Rut, User = user, CurrentPage = 1, PageSize = 10 };

        // Act
        PagedData<Device> returnedDevices = _businessOwnerService.GetDevices(args);

        // Assert
        returnedDevices.Should().BeEquivalentTo(devices);
        _businessRepository.Verify(x => x.Get(business.Rut), Times.Once);
        _deviceRepository.Verify(x => x.GetPaged(It.Is<GetDevicesArgs>(a => a.RutFilter == business.Rut)), Times.Once);
    }

    #endregion

    #endregion
}
