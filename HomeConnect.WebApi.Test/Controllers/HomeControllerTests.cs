using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Models;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Home;
using Microsoft.AspNetCore.Http;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class HomeControllerTests
{
    private HomeController _controller = null!;
    private Mock<HttpContext> _httpContextMock = null!;
    private Mock<IHomeOwnerService> _homeOwnerService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
        _homeOwnerService = new Mock<IHomeOwnerService>(MockBehavior.Strict);
        _controller = new HomeController();
    }

    [TestMethod]
    public void CreateHome_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var request = new CreateHomeRequest
        {
            Address = "123 Main St",
            Latitude = 123.456,
            Longitude = 456.789,
            MaxMembers = 3
        };
        var user = new User("John", "Doe", "email@email.com", "Password@100",
            new Role { Name = "Admin", Permissions = new List<SystemPermission>() });
        var items = new Dictionary<object, object?>
        {
            {
                Item.UserLogged,
                user
            }
        };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        var home = new Home(user, request.Address, request.Latitude, request.Longitude, request.MaxMembers);
        var args = new CreateHomeArgs
        {
            HomeOwnerId = user.Id.ToString(),
            Address = request.Address,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            MaxMembers = request.MaxMembers
        };
        _homeOwnerService.Setup(x => x.Create(args)).Returns(home.Id);

        // Act
        var response = _controller.CreateHome(request);

        // Assert
        _homeOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.Id.Should().Be(home.Id.ToString());
    }
}
