using BusinessLogic;
using FluentAssertions;
using Moq;

namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public class DeviceTest
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
        const string type = "Camera";
        var secondaryPhotos =
            new string[] { "https://www.example.com/photo2.jpg", "https://www.example.com/photo3.jpg" };

        // Act
        var act = () => new Device(name, modelNumber, description, mainPhoto, secondaryPhotos, type);

        // Assert
        act.Should().NotThrow();
    }

    #endregion

    #endregion
}
