using BusinessLogic.Admins.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Models;
using BusinessLogic.Users.Services;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Admins;
using HomeConnect.WebApi.Controllers.Admins.Models;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class AdminControllerTests
{
    private Mock<IUserService> _userService = null!;
    private Mock<IAdminService> _adminService = null!;
    private AdminController _controller = null!;

    [TestInitialize]
    public void Initialize()
    {
        _userService = new Mock<IUserService>();
        _adminService = new Mock<IAdminService>();
        _controller = new AdminController(_userService.Object, _adminService.Object);
    }

    #region CreateAdmin

    [TestMethod]
    public void CreateAdmin_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var request = new CreateAdminRequest
        {
            Name = "John",
            Surname = "Doe",
            Email = "email@email.com",
            Password = "password"
        };
        var userModel = new CreateUserArgs
        {
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            Password = request.Password,
            Role = Role.Admin
        };
        var guid = Guid.NewGuid();

        _userService.Setup(x => x.CreateUser(userModel)).Returns(new User { Id = guid });

        // Act
        CreateAdminResponse response = _controller.CreateAdmin(request);

        // Assert
        _userService.VerifyAll();
        response.Should().NotBeNull();
        response.Id.Should().Be(guid.ToString());
    }

    #endregion

    #region DeleteAdmin

    [TestMethod]
    public void DeleteAdmin_WhenCalledWithValidRequest_ReturnsNoContentResponse()
    {
        // Arrange
        var guid = Guid.NewGuid().ToString();
        _adminService.Setup(x => x.DeleteAdmin(guid));

        // Act
        DeleteAdminResponse response = _controller.DeleteAdmin(guid);

        // Assert
        _userService.VerifyAll();
        response.Id.Should().Be(guid);
    }

    #endregion
}
