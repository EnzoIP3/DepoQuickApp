using BusinessLogic.HomeOwners.Entities;
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
        _controller = new HomeOwnerController(_userService.Object, _homeOwnerService.Object);

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

    #region NameHome
    #region Success
    [TestMethod]
    public void NameHome_WithValidRequest_ReturnsHomeId()
    {
        // Arrange
        var request = new NameHomeRequest
        {
            HomeId = Guid.NewGuid().ToString(),
            NewName = "New Home Name"
        };
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _homeOwnerService.Setup(x => x.NameHome(_user.Id, Guid.Parse(request.HomeId), request.NewName));

        // Act
        NameHomeResponse response = _controller.NameHome(request);

        // Assert
        _homeOwnerService.Verify(x => x.NameHome(_user.Id, Guid.Parse(request.HomeId), request.NewName), Times.Once);
        response.Should().NotBeNull();
        response.HomeId.Should().Be(request.HomeId);
    }
    #endregion

    #region Error

    [TestMethod]
    public void NameHome_WithInvalidNewName_ThrowsArgumentException()
    {
        // Arrange
        var request = new NameHomeRequest
        {
            HomeId = Guid.NewGuid().ToString(),
            NewName = string.Empty
        };
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);

        // Act & Assert
        var ex = Assert.ThrowsException<ArgumentException>(() => _controller.NameHome(request));
        Assert.AreEqual("NewName cannot be null or empty", ex.Message);
    }

    [TestMethod]
    public void NameHome_WithInvalidHomeId_ThrowsArgumentException()
    {
        // Arrange
        var request = new NameHomeRequest
        {
            HomeId = string.Empty,
            NewName = "New Home Name"
        };
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);

        // Act & Assert
        var ex = Assert.ThrowsException<ArgumentException>(() => _controller.NameHome(request));
        Assert.AreEqual("HomeId cannot be null or empty", ex.Message);
    }

    #endregion
    #endregion
}
