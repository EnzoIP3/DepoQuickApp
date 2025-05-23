using BusinessLogic.Auth.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test.Auth.Entities;

[TestClass]
public class TokenTests
{
    [TestMethod]
    public void Constructor_WhenCalled_CreatesInstance()
    {
        // Arrange
        var user = new User();

        // Act
        Func<Token> act = () => new Token(user);

        // Assert
        act.Should().NotThrow();
    }

    [TestMethod]
    public void IsExpired_WhenTokenIsExpired_ReturnsTrue()
    {
        // Arrange
        var user = new User();
        var token = new Token(user) { CreatedAt = DateTime.UtcNow.AddHours(-Token.DurationInHours) };

        // Act
        var result = token.IsExpired();

        // Assert
        result.Should().BeTrue();
    }
}
