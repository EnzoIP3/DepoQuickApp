using BusinessLogic;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public class DeviceTest
{
    private const string Name = "Name";
    private const int ModelNumber = 123;
    private const string Description = "Description";
    private const string MainPhoto = "https://www.example.com/photo1.jpg";
    private const string Type = "Camera";
    private readonly List<string> secondaryPhotos = ["https://www.example.com/photo2.jpg", "https://www.example.com/photo3.jpg"];
    private static readonly User _owner = new User("John", "Doe", "JohnDoe@example.com", "Password123!", new Role());
    private readonly Business business = new Business("RUTexample", "Business Name", _owner);

    #region Create

    #region Success

    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_CreatesInstance()
    {
        // Act
        var act = () => new Device(Name, ModelNumber, Description, MainPhoto, secondaryPhotos, Type, business);

        // Assert
        act.Should().NotThrow();
    }

    #endregion

    #region Error

    [TestMethod]
    [DataRow("", 123, "Description", "https://www.example.com/photo1.jpg", "Camera")]
    [DataRow("Name", 0, "", "https://www.example.com/photo1.jpg", "Camera")]
    [DataRow("Name", 0, "Description", "", "Camera")]
    [DataRow("Name", 0, "Description", "https://www.example.com/photo1.jpg", "")]
    public void Constructor_WhenArgumentsAreBlank_ThrowsException(string name, int modelNumber, string description,
        string mainPhoto, string type)
    {
        // Act
        var act = () => new Device(name, modelNumber, description, mainPhoto, [], type, business);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_WhenMainPhotoFormatIsInvalid_ThrowsException()
    {
        // Arrange
        const string mainPhoto = "photo1.jpg";

        // Act
        var act = () => new Device(Name, ModelNumber, Description, mainPhoto, secondaryPhotos, Type, business);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_WhenSecondaryPhotoFormatIsInvalid_ThrowsException()
    {
        // Arrange
        var secondaryPhotos = new List<string> { "photo2.jpg", "https://www.example.com/photo3.jpg" };

        // Act
        var act = () => new Device(Name, ModelNumber, Description, MainPhoto, secondaryPhotos, Type, business);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #endregion
}
