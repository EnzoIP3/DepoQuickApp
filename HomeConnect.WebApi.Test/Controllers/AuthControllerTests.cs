using BusinessLogic.Auth.Entities;
using BusinessLogic.Auth.Models;
using BusinessLogic.Auth.Services;
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
        _tokenService = new Mock<IAuthService>();
        _controller = new AuthController(_tokenService.Object);
    }

    [TestMethod]
    public void CreateToken_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var request = new CreateTokenRequest { Email = "email", Password = "password" };
        var args = new CreateTokenArgs { Email = request.Email, Password = request.Password };
        var token = new Token(new User());
        _tokenService.Setup(x => x.CreateToken(args)).Returns(token.Id.ToString());

        // Act
        CreateTokenResponse response = _controller.CreateToken(request);

        // Assert
        _tokenService.Verify(x => x.CreateToken(args), Times.Once);
        response.Token.Should().Be(token.Id.ToString());
    }
}
