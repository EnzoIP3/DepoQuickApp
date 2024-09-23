using BusinessLogic;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public class UserTests
{
    private string _name = null!;
    private string _surname = null!;
    private string _email = null!;
    private string _password = null!;
    private string _role = null!;

    [TestInitialize]
    public void Initialize()
    {
        _name = "name";
        _surname = "surname";
        _email = "email@email.com";
        _password = "Password#100";
        _role = "Admin";
    }

    #region Constructor
    #region Error
    [TestMethod]
    [DataRow("", "surname", "email@email.com", "Password#100", "Admin")]
    [DataRow("name", "", "email@email.com", "Password#100", "Admin")]
    [DataRow("name", "surname", "", "Password#100", "Admin")]
    [DataRow("name", "surname", "email@email.com", "", "Admin")]
    [DataRow("name", "surname", "email@email.com", "Password#100", "")]
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
        // Act
        var act = () => new User(_name, _surname, "email.com", _password, _role);

        // Assert
        act.Should().Throw<Exception>().WithMessage("Email format invalid.");
    }

    [TestMethod]
    public void Constructor_WhenPasswordHasNoCapitalLetter_ThrowsException()
    {
        // Act
        var act = () => new User(_name, _surname, _email, "password100!", _role);

        // Assert
        act.Should().Throw<Exception>().WithMessage("Password must contain at least one capital letter.");
    }
    #endregion
    #region Success

    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_SetsProperties()
    {
        // Act
        var admin = new User(_name, _surname, _email, _password, _role);

        // Assert
        admin.Name.Should().Be(_name);
        admin.Surname.Should().Be(_surname);
        admin.Email.Should().Be(_email);
        admin.Password.Should().Be(_password);
    }
    #endregion
    #endregion
}
