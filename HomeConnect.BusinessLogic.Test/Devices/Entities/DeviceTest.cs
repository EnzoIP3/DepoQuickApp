using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test.Devices;

[TestClass]
public class DeviceTest
{
    private const string Name = "Name";
    private const int ModelNumber = 123;
    private const string Description = "Description";
    private const string MainPhoto = "https://www.example.com/photo1.jpg";
    private const string Type = "Camera";
    private readonly List<string> secondaryPhotos = ["https://www.example.com/photo2.jpg", "https://www.example.com/photo3.jpg"];
    private static readonly global::BusinessLogic.Users.Entities.User _owner = new global::BusinessLogic.Users.Entities.User("John", "Doe", "JohnDoe@example.com", "Password123!", new global::BusinessLogic.Roles.Entities.Role());
    private readonly Business business = new Business("RUTexample", "Business Name", "https://example.com/image.png", _owner);

    #region Create

    #region Success

    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_CreatesInstance()
    {
        // Act
        var act = () => new global::BusinessLogic.Devices.Entities.Device(Name, ModelNumber, Description, MainPhoto, secondaryPhotos, DeviceType.Camera.ToString(), business);

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
        var act = () => new global::BusinessLogic.Devices.Entities.Device(name, modelNumber, description, mainPhoto, [], type.ToString(), business);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_WhenMainPhotoFormatIsInvalid_ThrowsException()
    {
        // Arrange
        const string mainPhoto = "photo1.jpg";

        // Act
        var act = () => new global::BusinessLogic.Devices.Entities.Device(Name, ModelNumber, Description, mainPhoto, secondaryPhotos, Type, business);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_WhenSecondaryPhotoFormatIsInvalid_ThrowsException()
    {
        // Arrange
        var secondaryPhotos = new List<string> { "photo2.jpg", "https://www.example.com/photo3.jpg" };

        // Act
        var act = () => new global::BusinessLogic.Devices.Entities.Device(Name, ModelNumber, Description, MainPhoto, secondaryPhotos, Type, business);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #endregion
}
