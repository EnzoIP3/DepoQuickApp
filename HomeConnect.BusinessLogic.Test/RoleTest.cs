using BusinessLogic;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public class RoleTest
{
    private const string RoleName = "Admin";
    private const string Permission = "create_admin";
    private List<SystemPermission> _permissions = null!;
    private Role _role = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _permissions = [new SystemPermission(Permission)];
        _role = new Role(RoleName, _permissions);
    }

    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_CreatesInstance()
    {
        // Arrange

        // Act
        var act = () => new Role(RoleName, _permissions);

        // Assert
        act.Should().NotThrow();
    }

    [TestMethod]
    public void HasPermission_WhenCalledWithExistingPermission_ReturnsTrue()
    {
        // Arrange

        // Act
        var result = _role.HasPermission(Permission);

        // Assert
        result.Should().Be(true);
    }

    [TestMethod]
    public void HasPermission_WhenCalledWithNotExistingPermission_ReturnsFalse()
    {
        // Arrange
        var roleWithNoPermissions = new Role(RoleName, []);

        // Act
        var result = roleWithNoPermissions.HasPermission(Permission);

        // Assert
        result.Should().Be(false);
    }
}
