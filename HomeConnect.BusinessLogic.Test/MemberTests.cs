using BusinessLogic;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public class MemberTests
{
    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_CreatesInstance()
    {
        // Arrange
        var user = new User();

        // Act
        var act = () => new Member(user);

        // Assert
        act.Should().NotThrow();
    }
}
