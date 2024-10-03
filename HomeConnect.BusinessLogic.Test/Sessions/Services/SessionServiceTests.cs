using BusinessLogic.Roles.Entities;
using BusinessLogic.Sessions.Entities;
using BusinessLogic.Sessions.Models;
using BusinessLogic.Sessions.Repositories;
using BusinessLogic.Sessions.Services;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Repositories;
using FluentAssertions;
using Moq;

namespace HomeConnect.BusinessLogic.Test.Sessions.Services;

[TestClass]
public class SessionServiceTests
{
    private Mock<ISessionRepository> _sessionRepository = null!;
    private Mock<IUserRepository> _userRepository = null!;
    private ISessionService _sessionService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _sessionRepository = new Mock<ISessionRepository>();
        _userRepository = new Mock<IUserRepository>();
        _sessionService = new SessionService(_sessionRepository.Object, _userRepository.Object);
    }

    [TestMethod]
    public void GetUserFromSession_WithValidSessionId_ShouldReturnUser()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var user = new User();
        _sessionRepository.Setup(x => x.Get(sessionId)).Returns(new Session(user));

        // Act
        var act = () => _sessionService.GetUserFromSession(sessionId.ToString());

        // Assert
        act.Should().NotThrow();
    }

    [TestMethod]
    public void GetUserFromSession_WithInvalidSessionId_ShouldThrowException()
    {
        // Arrange
        const string sessionId = "not-a-guid";

        // Act
        var act = () => _sessionService.GetUserFromSession(sessionId);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void CreateSession_WithValidArguments_ShouldCreateSession()
    {
        // Arrange
        var args = new CreateSessionArgs() { Email = "test@example.com", Password = "password1M@" };
        var user = new User("Name", "Surname", args.Email, args.Password, new Role());
        _sessionRepository.Setup(x => x.Add(It.IsAny<Session>())).Verifiable();
        _userRepository.Setup(x => x.GetUser(args.Email)).Returns(user);

        // Act
        var result = _sessionService.CreateSession(args);

        // Assert
        result.Should().NotBeNullOrEmpty();
        _sessionRepository.Verify(x => x.Add(It.IsAny<Session>()), Times.Once);
    }

    [TestMethod]
    public void CreateSession_WithNonExistingEmail_ShouldThrowException()
    {
        // Arrange
        var args = new CreateSessionArgs() { Email = "test@example.com", Password = "password1M@" };
        _userRepository.Setup(x => x.GetUser(args.Email)).Returns((User)null);

        // Act
        var act = () => _sessionService.CreateSession(args);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void CreateSession_WithInvalidPassword_ShouldThrowException()
    {
        // Arrange
        var args = new CreateSessionArgs() { Email = "test@example.com", Password = "password" };
        var user = new User("Name", "Surname", args.Email, "otherPassword1@", new Role());
        _userRepository.Setup(x => x.GetUser(args.Email)).Returns(user);

        // Act
        var act = () => _sessionService.CreateSession(args);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void IsSessionExpired_WithValidSessionId_ShouldReturnFalse()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        _sessionRepository.Setup(x => x.Get(sessionId)).Returns(new Session(new User()));

        // Act
        var result = _sessionService.IsSessionExpired(sessionId.ToString());

        // Assert
        result.Should().BeFalse();
    }
}
