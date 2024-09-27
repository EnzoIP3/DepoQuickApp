using BusinessLogic;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public class RoleTest
{
    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_CreatesInstance()
    {
        // Arrange
        const string name = "Admin";
        var permissions = new List<SystemPermission> { new SystemPermission("create_admin") };

        // Act
        var act = () => new Role(name, permissions);

        // Assert
        act.Should().NotThrow();
    }

    [TestMethod]
    public void HasPermission_WhenCalledWithExistingPermission_ReturnsTrue()
    {
        // Arrange
        const string permission = "create_admin";
        var permissions = new List<SystemPermission> { new SystemPermission(permission) };
        var role = new Role("Admin", permissions);

        // Act
        var result = role.HasPermission(permission);

        // Assert
        result.Should().Be(true);
    }

    [TestMethod]
    public void HasPermission_WhenCalledWithNotExistingPermission_ReturnsFalse()
    {
        // Arrange
        const string permission = "create_admin";
        var permissions = new List<SystemPermission>();
        var role = new Role("Admin", permissions);

        // Act
        var result = role.HasPermission(permission);

        // Assert
        result.Should().Be(false);
    }
}
