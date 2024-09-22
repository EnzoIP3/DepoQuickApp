using BusinessLogic;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public class UserTests
{
    #region Constructor
    #region Error
    [TestMethod]
    [DataRow("", "surname", "email@email.com", "password", "Admin")]
    [DataRow("name", "", "email@email.com", "password", "Admin")]
    [DataRow("name", "surname", "", "password", "Admin")]
    [DataRow("name", "surname", "email@email.com", "", "Admin")]
    [DataRow("name", "surname", "email@email.com", "password", "")]
    public void Constructor_WhenArgumentsAreBlank_ThrowsException(string name, string surname, string email, string password, string role)
    {
        // Act
        var act = () => new User(name, surname, email, password, role);

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
        var role = "Admin";

        // Act
        var act = () => new User(name, surname, email, password, role);

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
        var role = "Admin";

        // Act
        var Admin = new User(name, surname, email, password, role);

        // Assert
        Admin.Name.Should().Be(name);
        Admin.Surname.Should().Be(surname);
        Admin.Email.Should().Be(email);
        Admin.Password.Should().Be(password);
    }

    #endregion
    #endregion
}
