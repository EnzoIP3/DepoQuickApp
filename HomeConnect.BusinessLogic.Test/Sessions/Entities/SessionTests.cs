using BusinessLogic.Sessions.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test.Sessions.Entities;

[TestClass]
public class SessionTests
{
    [TestMethod]
    public void Constructor_WithValidArguments_ShouldCreateInstance()
    {
        // Arrange
        var user = new User();

        // Act
        var act = () => new Session(user);

        // Assert
        act.Should().NotThrow();
    }

    [TestMethod]
    public void IsExpired_WithSessionExpired_ShouldReturnTrue()
    {
        // Arrange
        var user = new User();
        var session = new Session(user) { CreatedAt = DateTime.UtcNow.AddHours(-Session.DurationInHours) };

        // Act
        var result = session.IsExpired();

        // Assert
        result.Should().BeTrue();
    }
}
