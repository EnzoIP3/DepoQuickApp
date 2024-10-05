using BusinessLogic.HomeOwners.Entities;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test.HomeOwners.Entities;

[TestClass]
public class MemberTests
{
    #region Constructor

    #region Success

    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_CreatesInstance()
    {
        // Arrange
        var user = new global::BusinessLogic.Users.Entities.User();

        // Act
        var act = () => new Member(user);

        // Assert
        act.Should().NotThrow();
    }

    [TestMethod]
    public void Constructor_WhenHomePermissionsAreSet_CreatesInstance()
    {
        // Arrange
        var user = new global::BusinessLogic.Users.Entities.User();
        var permissions = new List<HomePermission> { new HomePermission("value") };

        // Act
        var act = () => new Member(user, permissions);

        // Assert
        act.Should().NotThrow();
    }

    #endregion

    #endregion

    #region AddPermission

    #region Success

    [TestMethod]
    public void AddPermission_WhenPermissionIsValid_AddsPermission()
    {
        // Arrange
        var user = new global::BusinessLogic.Users.Entities.User();
        var member = new Member(user);
        var permission = new HomePermission("value");

        // Act
        member.AddPermission(permission);

        // Assert
        member.HomePermissions.Should().Contain(permission);
    }

    #endregion

    #region Error

    [TestMethod]
    public void AddPermission_WhenPermissionIsAlreadyAdded_ThrowsInvalidOperationException()
    {
        // Arrange
        var user = new global::BusinessLogic.Users.Entities.User();
        var member = new Member(user);
        var permission = new HomePermission("value");
        member.AddPermission(permission);

        // Act
        var act = () => member.AddPermission(permission);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    #endregion

    #endregion

    #region DeletePermission

    #region Success

    [TestMethod]
    public void DeletePermission_WhenPermissionExists_DeletesPermission()
    {
        // Arrange
        var user = new global::BusinessLogic.Users.Entities.User();
        var member = new Member(user);
        var permission = new HomePermission("value");
        member.AddPermission(permission);

        // Act
        member.DeletePermission(permission);

        // Assert
        member.HomePermissions.Should().NotContain(permission);
    }

    #endregion

    #region Error

    [TestMethod]
    public void DeletePermission_WhenPermissionDoesNotExist_ThrowsInvalidOperationException()
    {
        // Arrange
        var user = new global::BusinessLogic.Users.Entities.User();
        var member = new Member(user);
        var permission = new HomePermission("value");

        // Act
        var act = () => member.DeletePermission(permission);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #endregion

    #region HasPermission

    #region Success

    [TestMethod]
    public void HasPermission_WhenPermissionExists_ReturnsTrue()
    {
        // Arrange
        var user = new global::BusinessLogic.Users.Entities.User();
        var member = new Member(user);
        var permission = new HomePermission("value");
        member.AddPermission(permission);

        // Act
        var result = member.HasPermission(permission);

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #endregion
}
