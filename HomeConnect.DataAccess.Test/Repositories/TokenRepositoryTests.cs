using BusinessLogic.Auth.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.DataAccess.Repositories;

namespace HomeConnect.DataAccess.Test.Repositories;

[TestClass]
public class TokenRepositoryTests
{
    private readonly Context _context = DbContextBuilder.BuildTestDbContext();
    private TokenRepository _tokenRepository = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();
        _tokenRepository = new TokenRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void Add_WithValidToken_AddsToken()
    {
        // Arrange
        var user = new User();
        var token = new Token(user);

        // Act
        _tokenRepository.Add(token);

        // Assert
        _context.Tokens.Find(token.Id).Should().NotBeNull();
    }

    [TestMethod]
    public void Get_WithValidToken_ReturnsToken()
    {
        // Arrange
        var user = new User();
        var token = new Token(user);
        _tokenRepository.Add(token);

        // Act
        Token result = _tokenRepository.Get(token.Id);

        // Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void Get_WithInvalidToken_ThrowsException()
    {
        // Arrange
        var user = new User();
        var token = new Token(user);
        _tokenRepository.Add(token);

        // Act
        Action act = () => _tokenRepository.Get(Guid.NewGuid());

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Exists_WhenTokenExists_ReturnsTrue()
    {
        // Arrange
        var user = new User();
        var token = new Token(user);
        _tokenRepository.Add(token);

        // Act
        var result = _tokenRepository.Exists(token.Id);

        // Assert
        result.Should().BeTrue();
    }
}
