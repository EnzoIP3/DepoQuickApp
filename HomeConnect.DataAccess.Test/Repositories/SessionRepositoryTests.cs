using BusinessLogic.Sessions.Entities;
using BusinessLogic.Sessions.Repositories;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.DataAccess.Repositories;

namespace HomeConnect.DataAccess.Test.Repositories;

[TestClass]
public class SessionRepositoryTests
{
    private readonly Context _context = DbContextBuilder.BuildTestDbContext();
    private SessionRepository _sessionRepository = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();
        _sessionRepository = new SessionRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void Add_WithValidSession_ShouldAddSession()
    {
        // Arrange
        var user = new User();
        var session = new Session(user);

        // Act
        _sessionRepository.Add(session);

        // Assert
        _context.Sessions.Find(session.Id).Should().NotBeNull();
    }

    [TestMethod]
    public void Get_WithValidSessionId_ShouldReturnSession()
    {
        // Arrange
        var user = new User();
        var session = new Session(user);
        _sessionRepository.Add(session);

        // Act
        var result = _sessionRepository.Get(session.Id);

        // Assert
        result.Should().NotBeNull();
    }
}
