using BusinessLogic.Auth.Entities;
using BusinessLogic.Auth.Models;
using BusinessLogic.Auth.Repositories;
using BusinessLogic.Auth.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Repositories;
using FluentAssertions;
using Moq;

namespace HomeConnect.BusinessLogic.Test.Auth.Services;

[TestClass]
public class AuthServiceTests
{
    private Mock<ITokenRepository> _tokenRepository = null!;
    private Mock<IUserRepository> _userRepository = null!;
    private IAuthService _authService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _tokenRepository = new Mock<ITokenRepository>();
        _userRepository = new Mock<IUserRepository>();
        _authService = new AuthService(_tokenRepository.Object, _userRepository.Object);
    }

    [TestMethod]
    public void GetUserFromToken_WithValidToken_ShouldReturnUser()
    {
        // Arrange
        var token = Guid.NewGuid();
        var user = new User();
        _tokenRepository.Setup(x => x.Get(token)).Returns(new Token(user));

        // Act
        var act = () => _authService.GetUserFromToken(token.ToString());

        // Assert
        act.Should().NotThrow();
    }

    [TestMethod]
    public void GetUserFromToken_WithInvalidToken_ShouldThrowException()
    {
        // Arrange
        const string token = "not-a-guid";

        // Act
        var act = () => _authService.GetUserFromToken(token);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void CreateToken_WithValidArguments_ShouldCreateToken()
    {
        // Arrange
        var args = new CreateTokenArgs() { Email = "test@example.com", Password = "password1M@" };
        var user = new User("Name", "Surname", args.Email, args.Password, new Role());
        _tokenRepository.Setup(x => x.Add(It.IsAny<Token>())).Verifiable();
        _userRepository.Setup(x => x.Get(args.Email)).Returns(user);

        // Act
        var result = _authService.CreateToken(args);

        // Assert
        result.Should().NotBeNullOrEmpty();
        _tokenRepository.Verify(x => x.Add(It.IsAny<Token>()), Times.Once);
    }

    [TestMethod]
    public void CreateToken_WithNonExistingEmail_ShouldThrowException()
    {
        // Arrange
        var args = new CreateTokenArgs() { Email = "test@example.com", Password = "password1M@" };
        _userRepository.Setup(x => x.Get(args.Email)).Returns((User)null);

        // Act
        var act = () => _authService.CreateToken(args);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void CreateToken_WithInvalidPassword_ShouldThrowException()
    {
        // Arrange
        var args = new CreateTokenArgs() { Email = "test@example.com", Password = "password" };
        var user = new User("Name", "Surname", args.Email, "otherPassword1@", new Role());
        _userRepository.Setup(x => x.Get(args.Email)).Returns(user);

        // Act
        var act = () => _authService.CreateToken(args);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void IsTokenExpired_WithValidToken_ShouldReturnFalse()
    {
        // Arrange
        var token = Guid.NewGuid();
        _tokenRepository.Setup(x => x.Get(token)).Returns(new Token(new User()));

        // Act
        var result = _authService.IsTokenExpired(token.ToString());

        // Assert
        result.Should().BeFalse();
    }
}
