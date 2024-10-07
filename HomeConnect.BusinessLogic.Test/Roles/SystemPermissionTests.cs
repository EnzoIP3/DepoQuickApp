using BusinessLogic.Roles.Entities;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test.Roles;

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
        Func<SystemPermission> act = () => new SystemPermission(value);

        // Assert
        act.Should().NotThrow();
    }

    #endregion

    #endregion
}
