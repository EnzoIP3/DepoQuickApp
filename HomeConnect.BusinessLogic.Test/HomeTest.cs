using BusinessLogic;

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
        Assert.IsNotNull(home);
    }
}
