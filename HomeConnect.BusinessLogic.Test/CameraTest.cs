using BusinessLogic;
using FluentAssertions;

namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public class CameraTest
{
    #region Create

    #region Success

    [TestMethod]
    public void Constructor_WhenArgumentsAreValid_CreatesInstance()
    {
        // Arrange
        const string name = "Name";
        const int modelNumber = 123;
        const string description = "Description";
        const string mainPhoto = "https://www.example.com/photo1.jpg";
        var secondaryPhotos =
            new List<string> { "https://www.example.com/photo2.jpg", "https://www.example.com/photo3.jpg" };
        const bool motionDetection = true;
        const bool personDetection = false;
        const bool isExterior = true;
        const bool isInterior = false;
        var _owner = new User("John", "Doe", "JohnDoe@example.com", "Password123!", new Role());
        var business = new Business("RUTexample", "Business Name", _owner);

        // Act
        var act = () => new Camera(name, modelNumber, description, mainPhoto, secondaryPhotos, business, motionDetection,
            personDetection, isExterior, isInterior);

        // Assert
        act.Should().NotThrow();
    }

    #endregion

    #region Error

    [TestMethod]
    public void Constructor_WhenNotExteriorAndNotInterior_ThrowsArgumentException()
    {
        // Arrange
        const string name = "Name";
        const int modelNumber = 123;
        const string description = "Description";
        const string mainPhoto = "https://www.example.com/photo1.jpg";
        var secondaryPhotos =
            new List<string> { "https://www.example.com/photo2.jpg", "https://www.example.com/photo3.jpg" };
        const bool motionDetection = true;
        const bool personDetection = false;
        const bool isExterior = false;
        const bool isInterior = false;
        var _owner = new User("John", "Doe", "JohnDoe@example.com", "Password123!", new Role());
        var business = new Business("RUTexample", "Business Name", _owner);

        // Act
        var act = () => new Camera(name, modelNumber, description, mainPhoto, secondaryPhotos, business, motionDetection,
            personDetection, isExterior, isInterior);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #endregion
}
