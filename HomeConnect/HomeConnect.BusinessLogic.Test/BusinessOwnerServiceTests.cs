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
    private BusinessOwnerService _businessOwnerService = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
        _businessRepository = new Mock<IBusinessRepository>(MockBehavior.Strict);
        _roleRepository = new Mock<IRoleRepository>(MockBehavior.Strict);
        _businessOwnerService = new BusinessOwnerService(_userRepository.Object, _businessRepository.Object, _roleRepository.Object);
    }

    #region CreateBusiness

    #region Success

    [TestMethod]
    public void CreateBusiness_WhenOwnerExists_CreatesBusiness()
    {
        // Arrange
        var ownerEmail = "owner@example.com";
        var businessRut = "123456789";
        var businessName = "Test Business";
        var owner = new User("John", "Doe", ownerEmail, "Password123!", new Role());

        _userRepository.Setup(x => x.GetUser(ownerEmail)).Returns(owner);
        _businessRepository.Setup(x => x.GetBusinessByOwner(ownerEmail)).Returns((Business?)null);
        _businessRepository.Setup(x => x.Add(It.IsAny<Business>()));
        _businessRepository.Setup(x => x.GetBusinessByRut(businessRut)).Returns((Business?)null);

        // Act
        _businessOwnerService.CreateBusiness(ownerEmail, businessRut, businessName);

        // Assert
        _businessRepository.Verify(x => x.Add(It.Is<Business>(b =>
            b.Rut == businessRut &&
            b.Name == businessName &&
            b.Owner.Email == ownerEmail)));
    }

    #endregion

    #region Failure

    [TestMethod]
    public void CreateBusiness_WhenOwnerAlreadyHasBusiness_ThrowsException()
    {
        // Arrange
        var ownerEmail = "owner@example.com";
        var businessRut = "123456789";
        var businessName = "Test Business";
        var owner = new User("John", "Doe", ownerEmail, "Password123!", new Role());
        var existingBusiness = new Business(businessRut, "Existing Business", owner);

        _userRepository.Setup(x => x.GetUser(ownerEmail)).Returns(owner);
        _businessRepository.Setup(x => x.GetBusinessByOwner(ownerEmail)).Returns(existingBusiness);

        // Act
        Action act = () => _businessOwnerService.CreateBusiness(ownerEmail, businessRut, businessName);

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Owner already has a business");
        _businessRepository.Verify(x => x.Add(It.IsAny<Business>()), Times.Never);
    }

    [TestMethod]
    public void CreateBusiness_WhenOwnerDoesNotExist_ThrowsException()
    {
        // Arrange
        var ownerEmail = "nonexistent@example.com";
        var businessRut = "123456789";
        var businessName = "Test Business";

        _userRepository.Setup(x => x.GetUser(ownerEmail)).Returns((User?)null);

        // Act
        Action act = () => _businessOwnerService.CreateBusiness(ownerEmail, businessRut, businessName);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Owner does not exist");
        _businessRepository.Verify(x => x.Add(It.IsAny<Business>()), Times.Never);
    }

    [TestMethod]
    public void CreateBusiness_WhenBusinessRutAlreadyExists_ThrowsException()
    {
        // Arrange
        var ownerEmail = "owner@example.com";
        var businessRut = "123456789";
        var businessName = "Test Business";
        var owner = new User("John", "Doe", ownerEmail, "Password123!", new Role());
        var existingBusiness = new Business(businessRut, "Existing Business", owner);

        _userRepository.Setup(x => x.GetUser(ownerEmail)).Returns(owner);
        _businessRepository.Setup(x => x.GetBusinessByRut(businessRut)).Returns(existingBusiness);
        _businessRepository.Setup(x => x.GetBusinessByOwner(ownerEmail)).Returns((Business?)null);
        _businessRepository.Setup(x => x.Add(It.IsAny<Business>()));
        _businessRepository.Setup(x=>x.GetBusinessByRut(businessRut)).Returns(existingBusiness);

        // Act
        Action act = () => _businessOwnerService.CreateBusiness(ownerEmail, businessRut, businessName);

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("RUT already exists");
        _businessRepository.Verify(x => x.Add(It.IsAny<Business>()), Times.Never);
    }

    #endregion

    #endregion
}
