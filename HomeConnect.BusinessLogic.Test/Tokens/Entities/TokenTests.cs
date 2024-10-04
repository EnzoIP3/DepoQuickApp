using BusinessLogic.Tokens.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test.Tokens.Entities;

[TestClass]
public class TokenTests
{
    [TestMethod]
    public void Constructor_WithValidArguments_ShouldCreateInstance()
    {
        // Arrange
        var user = new User();

        // Act
        var act = () => new Token(user);

        // Assert
        act.Should().NotThrow();
    }

    [TestMethod]
    public void IsExpired_WithExpiredToken_ShouldReturnTrue()
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
