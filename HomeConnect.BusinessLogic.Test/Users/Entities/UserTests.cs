using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test.Users.Entities;

[TestClass]
public class UserTests
{
    private const string Name = "name";
    private const string Surname = "surname";
    private const string Email = "email@email.com";
    private const string Password = "Password#100";

    private readonly Role _role =
        new Role("Admin", []);

    #region Constructor

    #region Error

    [TestMethod]
    [DataRow("", Surname, Email, Password)]
    [DataRow(Name, "", Email, Password)]
    [DataRow(Name, Surname, "", Password)]
    [DataRow(Name, Surname, Email, "")]
    public void Constructor_WhenArgumentsAreBlank_ThrowsException(string name, string surname, string email,
        string password)
    {
        // Act
        var act = () => new User(name, surname, email, password, _role);

        // Assert
        act.Should().Throw<Exception>().WithMessage("* cannot be blank.");
    }

    [TestMethod]
    public void Constructor_WhenEmailHasInvalidFormat_ThrowsException()
    {
        // Act
        var act = () => new User(Name, Surname, "email.com", Password, _role);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Email format invalid.");
    }

    [TestMethod]
    public void Constructor_WhenPasswordHasNoCapitalLetter_ThrowsException()
    {
        // Act
        var act = () => new User(Name, Surname, Email, "password100!", _role);

        // Assert
        act.Should().Throw<Exception>().WithMessage("Password must contain at least one capital letter.");
    }

    [TestMethod]
    public void Constructor_WhenPasswordHasNoDigit_ThrowsException()
    {
        // Act
        var act = () => new User(Name, Surname, Email, "Password!", _role);

        // Assert
        act.Should().Throw<Exception>().WithMessage("Password must contain at least one digit.");
    }

    [TestMethod]
    public void Constructor_WhenPasswordHasNoSpecialCharacter_ThrowsException()
    {
        // Act
        var act = () => new User(Name, Surname, Email, "Password100", _role);

        // Assert
        act.Should().Throw<Exception>().WithMessage("Password must contain at least one special character.");
    }

    [TestMethod]
    public void Constructor_WhenPasswordIsTooShort_ThrowsException()
    {
        // Act
        var act = () => new User(Name, Surname, Email, "Pwd1!", _role);

        // Assert
        act.Should().Throw<Exception>().WithMessage("Password must be at least 8 characters long.");
    }

    [TestMethod]
    public void Constructor_WhenProfilePictureIsNotAValidUrl_ThrowsException()
    {
        // Arrange
        const string profilePicture = "not-a-url";

        // Act
        var act = () => new User(Name, Surname, Email, Password, _role, profilePicture);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Profile picture must be a valid URL.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_SetsProperties()
    {
        // Act
        var admin = new User(Name, Surname, Email, Password, _role);

        // Assert
        admin.Name.Should().Be(Name);
        admin.Surname.Should().Be(Surname);
        admin.Email.Should().Be(Email);
        admin.Password.Should().Be(Password);
    }

    [TestMethod]
    public void Constructor_WhenProfilePictureIsProvided_SetsProfilePicture()
    {
        // Arrange
        const string profilePicture = "https://example.com/profile-picture.jpg";

        // Act
        var user = new User(Name, Surname, Email, Password, _role, profilePicture);

        // Assert
        user.ProfilePicture.Should().Be(profilePicture);
    }

    #endregion

    #endregion
}
