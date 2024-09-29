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

    [TestMethod]
    public void AddPermission_WhenPermissionIsValid_AddsPermission()
    {
        // Arrange
        var user = new User();
        var member = new Member(user);
        var permission = new HomePermission("value");

        // Act
        member.AddPermission(permission);

        // Assert
        member.HomePermissions.Should().Contain(permission);
    }

    [TestMethod]
    public void AddPermission_WhenPermissionIsAlreadyAdded_ThrowsInvalidOperationException()
    {
        // Arrange
        var user = new User();
        var member = new Member(user);
        var permission = new HomePermission("value");
        member.AddPermission(permission);

        // Act
        var act = () => member.AddPermission(permission);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void DeletePermission_WhenPermissionExists_DeletesPermission()
    {
        // Arrange
        var user = new User();
        var member = new Member(user);
        var permission = new HomePermission("value");
        member.AddPermission(permission);

        // Act
        member.DeletePermission(permission);

        // Assert
        member.HomePermissions.Should().NotContain(permission);
    }

    [TestMethod]
    public void DeletePermission_WhenPermissionDoesNotExist_ThrowsInvalidOperationException()
    {
        // Arrange
        var user = new User();
        var member = new Member(user);
        var permission = new HomePermission("value");

        // Act
        var act = () => member.DeletePermission(permission);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
}
