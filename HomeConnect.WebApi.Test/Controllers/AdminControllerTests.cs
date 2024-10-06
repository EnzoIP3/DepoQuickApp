using BusinessLogic.Admins.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Models;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Admin;
using HomeConnect.WebApi.Controllers.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class AdminControllerTests
{
    private Mock<IAdminService> _adminService = null!;
    private AdminController _controller = null!;

    [TestInitialize]
    public void Initialize()
    {
        _adminService = new Mock<IAdminService>();
        _controller = new AdminController(_adminService.Object);
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

        _adminService.Setup(x => x.Create(userModel)).Returns(guid);

        // Act
        var response = _controller.CreateAdmin(request);

        // Assert
        _adminService.VerifyAll();
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
        _adminService.Setup(x => x.Delete(guid));

        // Act
        var response = _controller.DeleteAdmin(guid);

        // Assert
        _adminService.VerifyAll();
        response.Should().NotBeNull();
        response.Should().BeOfType<NoContentResult>();
    }
    #endregion
}
