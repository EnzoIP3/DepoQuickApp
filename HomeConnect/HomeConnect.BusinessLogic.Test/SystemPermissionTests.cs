using BusinessLogic;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public class SystemPermissionTests
{
    #region Create

    #region Success

    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_CreatesInstance()
    {
        // Arrange
        const string value = "create_admin";

        // Act
        var act = () => new SystemPermission("create_admin");

        // Assert
        act.Should().NotThrow();
    }

    #endregion

    #endregion
}
