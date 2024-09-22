using BusinessLogic;
using FluentAssertions;
using Moq;

namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public sealed class AdminServiceTests
{
    private Mock<IAdminRepository> _adminRepository = null!;
    private Mock<IBusinessOwnerRepository> _businessOwnerRepository = null!;
    private AdminService _adminService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _adminRepository = new Mock<IAdminRepository>(MockBehavior.Strict);
        _businessOwnerRepository = new Mock<IBusinessOwnerRepository>(MockBehavior.Strict);
        _adminService = new AdminService(_adminRepository.Object, _businessOwnerRepository.Object);
    }
    #region Create
    #region Error
    [TestMethod]
    public void Create_WhenAlreadyExists_ThrowsException()
    {
        // Arrange
        var args = new UserModel
        {
            Name = "name",
            Surname = "surname",
            Email = "email",
            Password = "password",
            Role = "Admin"
        };
        _adminRepository.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);

        // Act
        var act = () => _adminService.Create(args);

        // Assert
        act.Should().Throw<Exception>().WithMessage("User already exists.");
    }

    [TestMethod]
    public void Create_WhenArgumentsHaveEmptyFields_ThrowsException()
    {
        // Arrange
        var args = new UserModel
        {
            Name = string.Empty,
            Surname = string.Empty,
            Email = string.Empty,
            Password = string.Empty,
            Role = string.Empty
        };

        // Act
        var act = () => _adminService.Create(args);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Invalid input data.");
    }
    #endregion
    #region Success
    [TestMethod]
    public void Create_WhenArgumentsAreValid_CreatesAdmin()
    {
        // Arrange
        var args = new UserModel
        {
            Name = "name",
            Surname = "surname",
            Email = "email@email.com",
            Password = "password",
            Role = "Admin"
        };
        _adminRepository.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);
        _adminRepository.Setup(x => x.Add(It.IsAny<User>()));

        // Act
        _adminService.Create(args);

        // Assert
        _adminRepository.Verify(x => x.Add(It.Is<User>(a =>
            a.Name == args.Name &&
            a.Surname == args.Surname &&
            a.Email == args.Email &&
            a.Password == args.Password &&
            a.Role.ToString() == args.Role)));
    }
    #endregion
    #endregion

    #region Delete
    #region Error
    [TestMethod]
    public void Delete_WhenDoesNotExist_ThrowsException()
    {
        // Arrange
        var args = "email";
        _adminRepository.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);

        // Act
        var act = () => _adminService.Delete(args);

        // Assert
        act.Should().Throw<Exception>().WithMessage("Admin does not exist.");
    }
    #endregion
    #region Success
    [TestMethod]
    public void Delete_WhenArgumentsAreValid_DeletesAdmin()
    {
        // Arrange
        var args = "email";
        _adminRepository.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
        _adminRepository.Setup(x => x.Delete(It.IsAny<string>()));

        // Act
        _adminService.Delete(args);

        // Assert
        _adminRepository.Verify(x => x.Delete(It.Is<string>(a => a == args)));
    }
    #endregion
    #endregion

    #region CreateBusinessOwner
    #region Error
    [TestMethod]
    public void CreateBusinessOwner_WhenAlreadyExists_ThrowsException()
    {
        // Arrange
        var args = new UserModel
        {
            Name = "name",
            Surname = "surname",
            Email = "email",
            Password = "password",
            Role = "BusinessOwner"
        };
        _businessOwnerRepository.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);

        // Act
        var act = () => _adminService.CreateBusinessOwner(args);

        // Assert
        act.Should().Throw<Exception>().WithMessage("User already exists.");
    }
    #endregion

    #region Success
    [TestMethod]
    public void CreateBusinessOwner_WhenArgumentsAreValid_CreatesBusinessOwner()
    {
        // Arrange
        var args = new UserModel
        {
            Name = "name",
            Surname = "surname",
            Email = "email@email.com",
            Password = "password",
            Role = "BusinessOwner"
        };
        _businessOwnerRepository.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);
        _businessOwnerRepository.Setup(x => x.Add(It.IsAny<User>()));

        // Act
        _adminService.CreateBusinessOwner(args);

        // Assert
        _businessOwnerRepository.Verify(x => x.Add(It.Is<User>(a =>
            a.Name == args.Name &&
            a.Surname == args.Surname &&
            a.Email == args.Email &&
            a.Password == args.Password &&
            a.Role.ToString() == args.Role)));
    }
    #endregion
    #endregion
}
