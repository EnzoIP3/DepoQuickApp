using BusinessLogic.Auth.Entities;
using BusinessLogic.Auth.Exceptions;
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
    private IAuthService _authService = null!;
    private Mock<ITokenRepository> _tokenRepository = null!;
    private Mock<IUserRepository> _userRepository = null!;

    [TestInitialize]
    public void Initialize()
    {
        _tokenRepository = new Mock<ITokenRepository>(MockBehavior.Strict);
        _userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
        _authService = new AuthService(_tokenRepository.Object, _userRepository.Object);
    }

    [TestMethod]
    public void GetUserFromToken_WhenTokenIsValid_ReturnsUser()
    {
        // Arrange
        var token = Guid.NewGuid();
        var user = new User();
        _tokenRepository.Setup(x => x.Get(token)).Returns(new Token(user));

        // Act
        Func<User> act = () => _authService.GetUserFromToken(token.ToString());

        // Assert
        act.Should().NotThrow();
    }

    [TestMethod]
    public void GetUserFromToken_WhenTokenIsInvalid_ThrowsException()
    {
        // Arrange
        const string token = "not-a-guid";

        // Act
        Func<User> act = () => _authService.GetUserFromToken(token);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void CreateToken_WhenArgumentsAreValid_CreatesToken()
    {
        // Arrange
        var args = new CreateTokenArgs { Email = "test@example.com", Password = "password1M@" };
        var user = new User("Name", "Surname", args.Email, args.Password, new Role());
        _tokenRepository.Setup(x => x.Add(It.IsAny<Token>())).Verifiable();
        _userRepository.Setup(x => x.ExistsByEmail(args.Email)).Returns(true);
        _userRepository.Setup(x => x.GetByEmail(args.Email)).Returns(user);

        // Act
        var result = _authService.CreateToken(args);

        // Assert
        result.Should().NotBeNullOrEmpty();
        _tokenRepository.Verify(x => x.Add(It.IsAny<Token>()), Times.Once);
    }

    [TestMethod]
    public void CreateToken_WhenEmailDoesNotExist_ThrowsException()
    {
        // Arrange
        var args = new CreateTokenArgs { Email = "test@example.com", Password = "password1M@" };
        _userRepository.Setup(x => x.ExistsByEmail(args.Email)).Returns(false);

        // Act
        Func<string> act = () => _authService.CreateToken(args);

        // Assert
        act.Should().Throw<AuthException>();
    }

    [TestMethod]
    public void CreateToken_WhenPasswordInvalid_ThrowsException()
    {
        // Arrange
        var args = new CreateTokenArgs { Email = "test@example.com", Password = "password" };
        var user = new User("Name", "Surname", args.Email, "otherPassword1@", new Role());
        _userRepository.Setup(x => x.ExistsByEmail(args.Email)).Returns(true);
        _userRepository.Setup(x => x.GetByEmail(args.Email)).Returns(user);

        // Act
        Func<string> act = () => _authService.CreateToken(args);

        // Assert
        act.Should().Throw<AuthException>();
    }

    [TestMethod]
    public void CreateToken_WhenEmailIsMissing_ThrowsException()
    {
        // Arrange
        var args = new CreateTokenArgs { Password = "password1M@" };

        // Act
        Func<string> act = () => _authService.CreateToken(args);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void CreateToken_WhenPasswordIsMissing_ThrowsException()
    {
        // Arrange
        var args = new CreateTokenArgs { Email = "test@example.com" };
        _userRepository.Setup(x => x.ExistsByEmail(args.Email)).Returns(true);
        _userRepository.Setup(x => x.GetByEmail(args.Email)).Returns(new User());

        // Act
        Func<string> act = () => _authService.CreateToken(args);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void IsTokenExpired_WhenTokenIsValid_ReturnsFalse()
    {
        // Arrange
        var token = Guid.NewGuid();
        _tokenRepository.Setup(x => x.Get(token)).Returns(new Token(new User()));

        // Act
        var result = _authService.IsTokenExpired(token.ToString());

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Exists_WhenTokenIsInvalid_ThrowsException()
    {
        // Arrange
        const string token = "guid";

        // Act
        Func<bool> act = () => _authService.Exists(token);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Exists_WhenTokenIsValid_ReturnsTrue()
    {
        // Arrange
        var token = Guid.NewGuid();
        _tokenRepository.Setup(x => x.Exists(token)).Returns(true);

        // Act
        var result = _authService.Exists(token.ToString());

        // Assert
        result.Should().BeTrue();
    }
}
