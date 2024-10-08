using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test.BusinessOwners.Entities;

[TestClass]
public class BusinessTests
{
    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_CreatesInstance()
    {
        // Arrange
        var user = new User();

        // Act
        Func<Business> act = () => new Business("RUT", "Business", "https://example.com/image.png", user);

        // Assert
        act.Should().NotThrow();
    }

    [TestMethod]
    public void Constructor_WhenInvalidLogoUrl_ThrowsException()
    {
        // Arrange
        var user = new User();

        // Act
        Func<Business> act = () => new Business("RUT", "Business", "not-a-url", user);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    [DataRow("", "Business", "https://example.com/image.png")]
    [DataRow("RUT", "", "https://example.com/image.png")]
    [DataRow("RUT", "Business", "")]
    public void Constructor_WhenArgumentsAreBlank_ThrowsException(string rut, string name, string logo)
    {
        // Arrange
        var user = new User();

        // Act
        Func<Business> act = () => new Business(rut, name, logo, user);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
