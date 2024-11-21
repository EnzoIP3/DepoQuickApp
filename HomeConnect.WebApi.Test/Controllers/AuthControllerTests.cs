using BusinessLogic.Auth.Entities;
using BusinessLogic.Auth.Models;
using BusinessLogic.Auth.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Auth;
using HomeConnect.WebApi.Controllers.Auth.Models;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class AuthControllerTests
{
    private AuthController _controller = null!;
    private Mock<IAuthService> _tokenService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _tokenService = new Mock<IAuthService>(MockBehavior.Strict);
        _controller = new AuthController(_tokenService.Object);
    }

    [TestMethod]
    public void CreateToken_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var request = new CreateTokenRequest { Email = "email", Password = "password" };
        var args = new CreateTokenArgs { Email = request.Email, Password = request.Password };
        var permissions = new List<SystemPermission>() { new("Permission") };
        var role = new Role() { Name = "Admin", Permissions = permissions };
        var user = new User() { Roles = [role] };
        var token = new Token();
        _tokenService.Setup(x => x.CreateToken(args)).Returns(token.Id.ToString());
        _tokenService.Setup(x => x.GetUserFromToken(token.Id.ToString())).Returns(user);

        // Act
        CreateTokenResponse response = _controller.CreateToken(request);

        // Assert
        _tokenService.Verify(x => x.CreateToken(args), Times.Once);
        response.Token.Should().Be(token.Id.ToString());
        response.Roles.Should()
            .BeEquivalentTo(
                new Dictionary<string, List<string>> { { role.Name, permissions.Select(x => x.Value).ToList() } });
        response.UserId.Should().Be(user.Id.ToString());
    }
}
