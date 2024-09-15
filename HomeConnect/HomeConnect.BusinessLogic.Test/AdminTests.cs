using BusinessLogic;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public class AdminTests
{
    #region Constructor
    #region Error
    [TestMethod]
    public void Constructor_WhenEmailIsBlank_ThrowsException()
    {
        // Arrange
        var username = "username";
        var surname = "surname";
        var email = string.Empty;
        var password = "password";

        // Act
        var act = () => new Admin(username, surname, email, password);

        // Assert
        act.Should().Throw<Exception>().WithMessage("Email is invalid.");
    }
    #endregion
    #endregion
}
