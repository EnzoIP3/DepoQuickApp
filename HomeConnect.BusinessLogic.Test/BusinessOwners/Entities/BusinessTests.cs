using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test.BusinessOwners.Entities;

[TestClass]
public class BusinessTests
{
    [TestMethod]
    public void Constructor_WithValidArguments_ShouldCreateInstance()
    {
        // Arrange
        var user = new User();

        // Act
        var act = () => new Business("RUT", "Business", "https://example.com/image.png", user);

        // Assert
        act.Should().NotThrow();
    }

    [TestMethod]
    public void Constructor_WithInvalidLogoUrl_ShouldThrowException()
    {
        // Arrange
        var user = new User();

        // Act
        var act = () => new Business("RUT", "Business", "not-a-url", user);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    [DataRow("", "Business", "https://example.com/image.png")]
    [DataRow("RUT", "", "https://example.com/image.png")]
    [DataRow("RUT", "Business", "")]
    public void Constructor_WithBlankArguments_ShouldThrowException(string rut, string name, string logo)
    {
        // Arrange
        var user = new User();

        // Act
        var act = () => new Business(rut, name, logo, user);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
