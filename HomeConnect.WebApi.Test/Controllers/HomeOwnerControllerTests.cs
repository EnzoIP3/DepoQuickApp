using BusinessLogic.HomeOwners.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Models;
using BusinessLogic.Users.Services;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.HomeOwners;
using HomeConnect.WebApi.Controllers.HomeOwners.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class HomeOwnerControllerTests
{
    private Mock<HttpContext> _httpContextMock = null!;
    private Mock<IUserService> _userService = null!;
    private Mock<IHomeOwnerService> _homeOwnerService = null!;
    private HomeOwnerController _controller = null!;
    private User _user = null!;

    [TestInitialize]
    public void Initialize()
    {
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
        _userService = new Mock<IUserService>(MockBehavior.Strict);
        _homeOwnerService = new Mock<IHomeOwnerService>(MockBehavior.Strict);
        _controller = new HomeOwnerController(_userService.Object);

        _user = new("John", "Doe", "email@email.com", "Password@100",
            new Role { Name = "HomeOwner", Permissions = [] });

        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = _httpContextMock.Object
        };
    }

    [TestMethod]
    public void CreateHomeOwner_WithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var request =
            new CreateHomeOwnerRequest
            {
                Name = "Name",
                Surname = "Surname",
                Email = "email",
                Password = "password",
                ProfilePicture = "profilePicture"
            };
        var args = new CreateUserArgs
        {
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            Password = request.Password,
            ProfilePicture = request.ProfilePicture,
            Role = Role.HomeOwner
        };
        var user = new User();
        _userService.Setup(x => x.CreateUser(args)).Returns(user);

        // Act
        CreateHomeOwnerResponse response = _controller.CreateHomeOwner(request);

        // Assert
        _userService.Verify(x => x.CreateUser(args), Times.Once);
        response.Id.Should().Be(user.Id.ToString());
    }
}
