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
    [DataRow("name", "", "email@email.com", "password")]
    [DataRow("name", "surname", "", "password")]
    [DataRow("name", "surname", "email@email.com", "")]
    public void Constructor_WhenArgumentsAreBlank_ThrowsException(string name, string surname, string email, string password)
    {
        // Act
        var act = () => new Admin(name, surname, email, password);

        // Assert
        act.Should().Throw<Exception>().WithMessage("Arguments cannot be blank.");
    }

    [TestMethod]
    public void Constructor_WhenEmailHasInvalidFormat_ThrowsException()
    {
        // Arrange
        var name = "name";
        var surname = "surname";
        var email = "email.com";
        var password = "password";

        // Act
        var act = () => new Admin(name, surname, email, password);

        // Assert
        act.Should().Throw<Exception>().WithMessage("Email format invalid.");
    }
    #endregion
    #region Success

    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_SetsProperties()
    {
        // Arrange
        var name = "name";
        var surname = "surname";
        var email = "email@email.com";
        var password = "password";

        // Act
        var admin = new Admin(name, surname, email, password);

        // Assert
        admin.Name.Should().Be(name);
        admin.Surname.Should().Be(surname);
        admin.Email.Should().Be(email);
        admin.Password.Should().Be(password);
    }

    #endregion
    #endregion
}
