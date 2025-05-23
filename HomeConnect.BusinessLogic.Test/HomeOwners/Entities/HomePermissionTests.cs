using BusinessLogic.HomeOwners.Entities;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test.HomeOwners.Entities;

[TestClass]
public class HomePermissionTests
{
    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_CreatesInstance()
    {
        // Arrange
        const string value = "value";

        // Act
        var result = new HomePermission(value);

        // Assert
        result.Should().NotBeNull();
    }
}
