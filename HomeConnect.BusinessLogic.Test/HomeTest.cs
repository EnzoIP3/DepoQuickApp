using BusinessLogic;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public class HomeTest
{
    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_CreatesInstance()
    {
        // Arrange
        var owner = new User();
        const string address = "Main St 123";
        const double latitude = 123.456;
        const double longitude = 456.789;
        const int maxMembers = 5;

        // Act
        var home = new Home(owner, address, latitude, longitude, maxMembers);

        // Assert
        home.Should().NotBeNull();
    }

    [TestMethod]
    [DataRow("Main St")]
    [DataRow("123")]
    [DataRow("123 Main St")]
    public void Constructor_WhenAddressIsNotRoadAndNumber_ThrowsArgumentException(string address)
    {
        // Arrange
        var owner = new User();
        const double latitude = 123.456;
        const double longitude = 456.789;
        const int maxMembers = 5;

        // Act
        var act = () => new Home(owner, address, latitude, longitude, maxMembers);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
