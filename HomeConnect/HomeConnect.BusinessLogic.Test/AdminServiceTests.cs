using BusinessLogic;
using FluentAssertions;
using Moq;

namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public sealed class AdminServiceTests
{
    private Mock<IAdminRepository> _adminRepository = null!;
    private AdminService _adminService = null!;
    [TestInitialize]
    public void Initialize()
    {
        _adminRepository = new Mock<IAdminRepository>(MockBehavior.Strict);
        _adminService = new AdminService(_adminRepository.Object);
    }
    #region Create
    #region Error
    [TestMethod]
    public void Create_WhenAlreadyExists_ThrowsException()
    {
        // Arrange
        var args = new AdminModel
        {
            Username = "username",
            Surname = "surname",
            Email = "email",
            Password = "password"
        };
        _adminRepository.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);

        // Act
        var act = () => _adminService.Create(args);

        // Assert
        act.Should().Throw<Exception>().WithMessage("Username already exists.");
    }

    #region Success
    [TestMethod]
    public void Create_WhenArgumentsAreValid_CreatesAdmin()
    {
        // Arrange
        var args = new AdminModel
        {
            Username = "username",
            Surname = "surname",
            Email = "email",
            Password = "password"
        };
        _adminRepository.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);
        _adminRepository.Setup(x => x.Add(It.IsAny<Admin>()));

        // Act
        _adminService.Create(args);

        // Assert
        _adminRepository.Verify(x => x.Add(It.Is<Admin>(a =>
            a.Username == args.Username &&
            a.Surname == args.Surname &&
            a.Email == args.Email &&
            a.Password == args.Password)));
    }
    #endregion
    #endregion
    #endregion
}
