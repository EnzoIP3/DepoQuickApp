using BusinessLogic.Roles.Entities;
using BusinessLogic.Tokens.Entities;
using BusinessLogic.Tokens.Models;
using BusinessLogic.Tokens.Repositories;
using BusinessLogic.Tokens.Services;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Repositories;
using FluentAssertions;
using Moq;

namespace HomeConnect.BusinessLogic.Test.Tokens.Services;

[TestClass]
public class TokenServiceTests
{
    private Mock<ITokenRepository> _tokenRepository = null!;
    private Mock<IUserRepository> _userRepository = null!;
    private ITokenService _tokenService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _tokenRepository = new Mock<ITokenRepository>();
        _userRepository = new Mock<IUserRepository>();
        _tokenService = new TokenService(_tokenRepository.Object, _userRepository.Object);
    }

    [TestMethod]
    public void GetUserFromToken_WithValidToken_ShouldReturnUser()
    {
        // Arrange
        var token = Guid.NewGuid();
        var user = new User();
        _tokenRepository.Setup(x => x.Get(token)).Returns(new Token(user));

        // Act
        var act = () => _tokenService.GetUserFromToken(token.ToString());

        // Assert
        act.Should().NotThrow();
    }

    [TestMethod]
    public void GetUserFromToken_WithInvalidToken_ShouldThrowException()
    {
        // Arrange
        const string token = "not-a-guid";

        // Act
        var act = () => _tokenService.GetUserFromToken(token);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void GetToken_WithValidArguments_ShouldCreateToken()
    {
        // Arrange
        var args = new GetTokenArgs() { Email = "test@example.com", Password = "password1M@" };
        var user = new User("Name", "Surname", args.Email, args.Password, new Role());
        _tokenRepository.Setup(x => x.Add(It.IsAny<Token>())).Verifiable();
        _userRepository.Setup(x => x.GetUser(args.Email)).Returns(user);

        // Act
        var result = _tokenService.GetToken(args);

        // Assert
        result.Should().NotBeNullOrEmpty();
        _tokenRepository.Verify(x => x.Add(It.IsAny<Token>()), Times.Once);
    }

    [TestMethod]
    public void GetToken_WithNonExistingEmail_ShouldThrowException()
    {
        // Arrange
        var args = new GetTokenArgs() { Email = "test@example.com", Password = "password1M@" };
        _userRepository.Setup(x => x.GetUser(args.Email)).Returns((User)null);

        // Act
        var act = () => _tokenService.GetToken(args);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void GetToken_WithInvalidPassword_ShouldThrowException()
    {
        // Arrange
        var args = new GetTokenArgs() { Email = "test@example.com", Password = "password" };
        var user = new User("Name", "Surname", args.Email, "otherPassword1@", new Role());
        _userRepository.Setup(x => x.GetUser(args.Email)).Returns(user);

        // Act
        var act = () => _tokenService.GetToken(args);

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
        var result = _tokenService.IsTokenExpired(token.ToString());

        // Assert
        result.Should().BeFalse();
    }
}
