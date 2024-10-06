using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test.Devices.Entities;

[TestClass]
public class DeviceTest
{
    private const string Name = "Name";
    private const int ModelNumber = 123;
    private const string Description = "Description";
    private const string MainPhoto = "https://www.example.com/photo1.jpg";
    private const string Type = "Camera";

    private readonly List<string> _secondaryPhotos =
        ["https://www.example.com/photo2.jpg", "https://www.example.com/photo3.jpg"];

    private static readonly User Owner = new User("John", "Doe", "JohnDoe@example.com", "Password123!", new Role());

    private readonly Business _business =
        new Business("RUTexample", "Business Name", "https://example.com/image.png", Owner);

    #region Create

    #region Success

    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_CreatesInstance()
    {
        // Act
        var act = () => new Device(Name, ModelNumber, Description, MainPhoto, _secondaryPhotos,
            DeviceType.Camera.ToString(), _business);

        // Assert
        act.Should().NotThrow();
    }

    #endregion

    #region Error

    [TestMethod]
    [DataRow("", 123, "Description", "https://www.example.com/photo1.jpg", DeviceType.Camera)]
    [DataRow("Name", 123, "", "https://www.example.com/photo1.jpg", DeviceType.Camera)]
    [DataRow("Name", 123, "Description", "", DeviceType.Camera)]
    public void Constructor_WhenArgumentsAreBlank_ThrowsException(string name, int modelNumber, string description,
        string mainPhoto, DeviceType type)
    {
        // Act
        var act = () => new Device(name, modelNumber, description, mainPhoto, [], type.ToString(), _business);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_WhenMainPhotoFormatIsInvalid_ThrowsException()
    {
        // Arrange
        const string mainPhoto = "photo1.jpg";

        // Act
        var act = () => new Device(Name, ModelNumber, Description, mainPhoto, _secondaryPhotos, Type, _business);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_WhenSecondaryPhotoFormatIsInvalid_ThrowsException()
    {
        // Arrange
        var secondaryPhotos = new List<string> { "photo2.jpg", "https://www.example.com/photo3.jpg" };

        // Act
        var act = () => new Device(Name, ModelNumber, Description, MainPhoto, secondaryPhotos, Type, _business);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_WhenModelNumberIsNull_ThrowsException()
    {
        // Act
        var act = () => new Device(Name, null, Description, MainPhoto, _secondaryPhotos, Type, _business);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #endregion
}
