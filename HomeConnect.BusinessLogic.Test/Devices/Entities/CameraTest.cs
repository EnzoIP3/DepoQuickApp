using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test.Devices;

[TestClass]
public class CameraTest
{
    private const string Name = "Name";
    private const int ModelNumber = 123;
    private const string Description = "Description";
    private const string MainPhoto = "https://www.example.com/photo1.jpg";

    private const bool MotionDetection = true;
    private const bool PersonDetection = false;
    private const bool IsExterior = true;
    private const bool IsInterior = false;

    private readonly List<string> secondaryPhotos =
        ["https://www.example.com/photo2.jpg", "https://www.example.com/photo3.jpg"];

    private Business _business = null!;
    private User _owner = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _owner = new User("John", "Doe", "JohnDoe@example.com", "Password123!",
            new Role());
        _business = new Business("RUTexample", "Business Name", "https://example.com/image.png", _owner);
    }

    #region Create

    #region Success

    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_CreatesInstance()
    {
        // Arrange

        // Act
        Func<Camera> act = () => new Camera(Name, ModelNumber, Description, MainPhoto, secondaryPhotos, _business,
            MotionDetection,
            PersonDetection, IsExterior, IsInterior);

        // Assert
        act.Should().NotThrow();
    }

    #endregion

    #region Error

    [TestMethod]
    public void Constructor_WhenNotExteriorAndNotInterior_ThrowsArgumentException()
    {
        // Arrange
        const bool isExterior = false;
        const bool isInterior = false;

        // Act
        Func<Camera> act = () => new Camera(Name, ModelNumber, Description, MainPhoto, secondaryPhotos, _business,
            MotionDetection,
            PersonDetection, isExterior, isInterior);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_WhenMotionDetectionIsNull_ThrowsArgumentException()
    {
        // Act
        Func<Camera> act = () => new Camera(Name, ModelNumber, Description, MainPhoto, secondaryPhotos, _business,
            null,
            PersonDetection, IsExterior, IsInterior);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_WhenPersonDetectionIsNull_ThrowsArgumentException()
    {
        // Act
        Func<Camera> act = () => new Camera(Name, ModelNumber, Description, MainPhoto, secondaryPhotos, _business,
            MotionDetection,
            null, IsExterior, IsInterior);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_WhenIsExteriorIsNull_ThrowsArgumentException()
    {
        // Act
        Func<Camera> act = () => new Camera(Name, ModelNumber, Description, MainPhoto, secondaryPhotos, _business,
            MotionDetection,
            PersonDetection, null, IsInterior);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_WhenIsInteriorIsNull_ThrowsArgumentException()
    {
        // Act
        Func<Camera> act = () => new Camera(Name, ModelNumber, Description, MainPhoto, secondaryPhotos, _business,
            MotionDetection,
            PersonDetection, IsExterior, null);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #endregion
}
