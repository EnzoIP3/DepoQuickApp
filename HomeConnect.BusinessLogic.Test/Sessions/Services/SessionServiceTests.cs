using BusinessLogic.Sessions.Entities;
using BusinessLogic.Sessions.Repositories;
using BusinessLogic.Sessions.Services;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using Moq;

namespace HomeConnect.BusinessLogic.Test.Sessions.Services;

[TestClass]
public class SessionServiceTests
{
    private Mock<ISessionRepository> _sessionRepository = null!;
    private ISessionService _sessionService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _sessionRepository = new Mock<ISessionRepository>();
        _sessionService = new SessionService(_sessionRepository.Object);
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
}
