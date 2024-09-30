using BusinessLogic;
using FluentAssertions;
namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public class CameraTest
{
    private const string Name = "Name";
    private const int ModelNumber = 123;
    private const string Description = "Description";
    private const string MainPhoto = "https://www.example.com/photo1.jpg";
    private readonly List<string> SecondaryPhotos = new List<string> { "https://www.example.com/photo2.jpg", "https://www.example.com/photo3.jpg" };
    private const bool MotionDetection = true;
    private const bool PersonDetection = false;
    private const bool IsExterior = true;
    private const bool IsInterior = false;
    private User _owner = null!;
    private Business _business = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _owner = new User("John", "Doe", "JohnDoe@example.com", "Password123!", new Role());
        _business = new Business("RUTexample", "Business Name", _owner);
    }

    #region Create

    #region Success

    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_CreatesInstance()
    {
        // Arrange

        // Act
        var act = () => new Camera(Name, ModelNumber, Description, MainPhoto, SecondaryPhotos, _business, MotionDetection,
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
        var act = () => new Camera(Name, ModelNumber, Description, MainPhoto, SecondaryPhotos, _business, MotionDetection,
            PersonDetection, isExterior, isInterior);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #endregion
}
