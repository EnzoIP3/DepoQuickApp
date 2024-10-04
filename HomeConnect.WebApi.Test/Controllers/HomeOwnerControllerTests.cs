using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Models;
using BusinessLogic.Users.Services;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.HomeOwner;
using HomeConnect.WebApi.Controllers.HomeOwner.Models;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class HomeOwnerControllerTests
{
    private Mock<IUserService> _userService = null!;
    private HomeOwnerController _controller = null!;

    [TestInitialize]
    public void Initialize()
    {
        _userService = new Mock<IUserService>();
        _controller = new HomeOwnerController(_userService.Object);
    }

    [TestMethod]
    public void CreateHomeOwner_WithValidRequest_ShouldReturnCreatedResponse()
    {
        // Arrange
        var request = new CreateHomeOwnerRequest { Email = "email", Password = "password" };
        var args = new CreateUserArgs { Email = request.Email, Password = request.Password };
        var user = new User();
        _userService.Setup(x => x.CreateUser(args)).Returns(user);

        // Act
        var response = _controller.CreateHomeOwner(request);

        // Assert
        _userService.Verify(x => x.CreateUser(args), Times.Once);
        response.Id.Should().Be(user.Id.ToString());
    }
}
