using BusinessLogic.Sessions.Entities;
using BusinessLogic.Users.Entities;

namespace HomeConnect.BusinessLogic.Test.Sessions;

[TestClass]
public class SessionTests
{
    [TestMethod]
    public void Constructor_WithValidArguments_ShouldCreateInstance()
    {
        // Arrange
        var user = new User();

        // Act
        var session = new Session(user);

        // Assert
        Assert.IsNotNull(session);
    }

    [TestMethod]
    public void IsExpired_WithSessionExpired_ShouldReturnTrue()
    {
        // Arrange
        var user = new User();
        var session = new Session(user) { CreatedAt = DateTime.UtcNow.AddHours(-1) };

        // Act
        var result = session.IsExpired();

        // Assert
        Assert.IsTrue(result);
    }
}
