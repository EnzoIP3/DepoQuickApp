using BusinessLogic;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public class AdminTests
{
    #region Constructor
    #region Error
    [TestMethod]
    [DataRow("", "surname", "email@email.com", "password")]
    [DataRow("username", "", "email@email.com", "password")]
    [DataRow("username", "surname", "", "password")]
    [DataRow("username", "surname", "email@email.com", "")]
    public void Constructor_WhenArgumentsAreBlank_ThrowsException(string username, string surname, string email, string password)
    {
        // Act
        var act = () => new Admin(username, surname, email, password);

        // Assert
        act.Should().Throw<Exception>().WithMessage("Arguments cannot be blank.");
    }

    [TestMethod]
    public void Constructor_WhenEmailHasInvalidFormat_ThrowsException()
    {
        // Arrange
        var username = "username";
        var surname = "surname";
        var email = "email.com";
        var password = "password";

        // Act
        var act = () => new Admin(username, surname, email, password);

        // Assert
        act.Should().Throw<Exception>().WithMessage("Email format invalid.");
    }
    #endregion
    #region Success

    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_SetsProperties()
    {
        // Arrange
        var username = "username";
        var surname = "surname";
        var email = "email@email.com";
        var password = "password";

        // Act
        var admin = new Admin(username, surname, email, password);

        // Assert
        admin.Username.Should().Be(username);
        admin.Surname.Should().Be(surname);
        admin.Email.Should().Be(email);
        admin.Password.Should().Be(password);
    }

    #endregion
    #endregion
}
