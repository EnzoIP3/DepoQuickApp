using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Models;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.Home;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class HomeControllerTests
{
    private HomeController _controller = null!;
    private Mock<HttpContext> _httpContextMock = null!;
    private Mock<IHomeOwnerService> _homeOwnerService = null!;
    private AuthorizationFilterContext _context = null!;

    [TestInitialize]
    public void Initialize()
    {
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
        _homeOwnerService = new Mock<IHomeOwnerService>(MockBehavior.Strict);
        _controller = new HomeController(_homeOwnerService.Object);
        _context = new AuthorizationFilterContext(
            new ActionContext(
                _httpContextMock.Object,
                new RouteData(),
                new ActionDescriptor()),
            new List<IFilterMetadata>());
    }

    #region CreateHome
    [TestMethod]
    public void CreateHome_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var request = new CreateHomeRequest
        {
            Address = "Road 123",
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
        var response = _controller.CreateHome(request, _context);

        // Assert
        _homeOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.Id.Should().Be(home.Id.ToString());
    }
    #endregion

    #region AddMember
    [TestMethod]
    public void AddMember_WhenCalledWithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var user = new User("John", "Doe", "email@email.com", "Password@100",
            new Role { Name = "Admin", Permissions = new List<SystemPermission>() });
        var home = new Home(user, "Road 123", 123.456, 456.789, 3);
        var request = new AddMemberRequest
        {
            HomeId = home.Id.ToString(),
            HomeOwnerId = user.Id.ToString(),
            CanAddDevices = true,
            CanListDevices = false
        };
        var items = new Dictionary<object, object?>
        {
            {
                Item.UserLogged,
                user
            }
        };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        var args = new AddMemberArgs
        {
            HomeId = home.Id.ToString(),
            HomeOwnerId = user.Id.ToString(),
            CanAddDevices = request.CanAddDevices,
            CanListDevices = request.CanListDevices
        };
        _homeOwnerService.Setup(x => x.AddMemberToHome(args)).Returns(user.Id);

        // Act
        var response = _controller.AddMember(home.Id.ToString(), request, _context);

        // Assert
        _homeOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.HomeId.Should().Be(home.Id.ToString());
        response.MemberId.Should().Be(user.Id.ToString());
    }
    #endregion
}
