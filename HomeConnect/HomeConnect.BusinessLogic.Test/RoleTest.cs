using BusinessLogic;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public class RoleTest
{
    #region Create

    #region Success

    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_CreatesInstance()
    {
        // Arrange
        var name = "Admin";
        var permissions = new List<SystemPermission> { new SystemPermission("create_admin") };

        // Act
        var act = () => new RoleClass(name, permissions);

        // Assert
        act.Should().NotThrow();
    }

    #endregion

    #endregion
}
