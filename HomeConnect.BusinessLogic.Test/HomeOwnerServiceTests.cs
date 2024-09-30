using BusinessLogic;
using FluentAssertions;
using Moq;

namespace HomeConnect.BusinessLogic.Test;

[TestClass]
public class HomeOwnerServiceTests
{
    private Mock<IHomeRepository> _homeRepositoryMock = null!;
    private Mock<IUserRepository> _userRepositoryMock = null!;
    private HomeOwnerService _homeOwnerService = null!;
    private User _user = new User("John", "Doe", "test@example.com", "12345678@My", new Role());

    [TestInitialize]
    public void Initialize()
    {
        _homeRepositoryMock = new Mock<IHomeRepository>(MockBehavior.Strict);
        _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        _homeOwnerService = new HomeOwnerService(_homeRepositoryMock.Object, _userRepositoryMock.Object);
    }

    [TestMethod]
    public void CreateHome_WhenHomeIsValid_AddsHome()
    {
        // Arrange
        var model = new CreateHomeModel
        {
            HomeOwnerEmail = _user.Email,
            Address = "Main St 123",
            Latitude = 1.0,
            Longitude = 2.0,
            MaxMembers = 5
        };
        _userRepositoryMock.Setup(x => x.Get(model.HomeOwnerEmail)).Returns(_user);
        _homeRepositoryMock.Setup(x => x.Add(It.IsAny<Home>())).Verifiable();

        // Act
        _homeOwnerService.CreateHome(model);

        // Assert
        _homeRepositoryMock.Verify(x => x.Add(It.IsAny<Home>()), Times.Once);
    }

    [TestMethod]
    [DataRow("", "Main St 123")]
    [DataRow("test@example.com", "")]
    public void CreateHome_WhenArgumentsHaveEmptyFields_ThrowsException(string homeOwnerEmail, string address)
    {
        // Arrange
        var model = new CreateHomeModel
        {
            HomeOwnerEmail = homeOwnerEmail,
            Address = address,
            Latitude = 1.0,
            Longitude = 2.0,
            MaxMembers = 5
        };

        // Act
        var act = () => _homeOwnerService.CreateHome(model);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
