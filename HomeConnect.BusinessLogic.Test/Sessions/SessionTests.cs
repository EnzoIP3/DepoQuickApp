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
}
