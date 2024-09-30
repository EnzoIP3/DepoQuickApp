using BusinessLogic;
using Moq;

namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public class HomeOwnerServiceTests
{
    private Mock<IHomeRepository> _homeRepositoryMock = null!;
    private HomeOwnerService _homeOwnerService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _homeRepositoryMock = new Mock<IHomeRepository>(MockBehavior.Strict);
        _homeOwnerService = new HomeOwnerService(_homeRepositoryMock.Object);
    }

    [TestMethod]
    public void CreateHome_WhenHomeIsValid_AddsHome()
    {
        // Arrange
        var home = new Home(new User(), "Address 123", 12.5, 12.5, 1);
        _homeRepositoryMock.Setup(x => x.Add(home));

        // Act
        _homeOwnerService.CreateHome(home);

        // Assert
        _homeRepositoryMock.Verify(x => x.Add(home), Times.Once);
    }
}
