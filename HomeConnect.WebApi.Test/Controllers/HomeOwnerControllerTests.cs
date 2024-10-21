using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Models;
using BusinessLogic.Users.Services;
using FluentAssertions;
using HomeConnect.WebApi.Controllers.HomeOwners;
using HomeConnect.WebApi.Controllers.HomeOwners.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HomeConnect.WebApi.Test.Controllers;

[TestClass]
public class HomeOwnerControllerTests
{
    private Mock<HttpContext> _httpContextMock = null!;
    private Mock<IUserService> _userService = null!;
    private Mock<IHomeOwnerService> _homeOwnerService = null!;
    private HomeOwnerController _controller = null!;
    private User _user = null!;

    [TestInitialize]
    public void Initialize()
    {
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
        _userService = new Mock<IUserService>(MockBehavior.Strict);
        _homeOwnerService = new Mock<IHomeOwnerService>(MockBehavior.Strict);
        _controller = new HomeOwnerController(_userService.Object, _homeOwnerService.Object);

        _user = new("John", "Doe", "email@email.com", "Password@100",
            new Role { Name = "HomeOwner", Permissions = [] });

        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = _httpContextMock.Object
        };
    }

    [TestMethod]
    public void CreateHomeOwner_WithValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var request =
            new CreateHomeOwnerRequest
            {
                Name = "Name",
                Surname = "Surname",
                Email = "email",
                Password = "password",
                ProfilePicture = "profilePicture"
            };
        var args = new CreateUserArgs
        {
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            Password = request.Password,
            ProfilePicture = request.ProfilePicture,
            Role = Role.HomeOwner
        };
        var user = new User();
        _userService.Setup(x => x.CreateUser(args)).Returns(user);

        // Act
        CreateHomeOwnerResponse response = _controller.CreateHomeOwner(request);

        // Assert
        _userService.Verify(x => x.CreateUser(args), Times.Once);
        response.Id.Should().Be(user.Id.ToString());
    }

    #region GetHomes
    [TestMethod]
    public void GetHomes_WhenCalledWithValidRequest_ReturnsHomes()
    {
        // Arrange
        var home1 = new Home
        {
            Id = Guid.NewGuid(),
            Address = "Amarales 3420",
            Latitude = 40.7128,
            Longitude = -74.0060,
            MaxMembers = 4
        };
        var home2 = new Home
        {
            Id = Guid.NewGuid(),
            Address = "Arteaga 1470",
            Latitude = 34.0522,
            Longitude = -118.2437,
            MaxMembers = 6
        };
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _homeOwnerService.Setup(x => x.GetHomesByOwnerId(_user.Id))
            .Returns(new List<Home> { home1, home2 });

        var expectedResponse = new GetHomesResponse
        {
            Homes = new List<ListHomeInfo>
            {
                new ListHomeInfo
                {
                    Id = home1.Id.ToString(),
                    Address = home1.Address,
                    Latitude = home1.Latitude,
                    Longitude = home1.Longitude,
                    MaxMembers = home1.MaxMembers
                },
                new ListHomeInfo
                {
                    Id = home2.Id.ToString(),
                    Address = home2.Address,
                    Latitude = home2.Latitude,
                    Longitude = home2.Longitude,
                    MaxMembers = home2.MaxMembers
                }
            }
        };

        // Act
        GetHomesResponse response = _controller.GetHomes(new GetHomesRequest());

        // Assert
        _homeOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.Homes.Should().NotBeNullOrEmpty();
        response.Homes.Should().HaveCount(2);
        response.Homes.Should().BeEquivalentTo(expectedResponse.Homes);
    }
    #endregion

    #region NameHome
    [TestMethod]
    public void NameHome_WithValidRequest_ReturnsHomeId()
    {
        // Arrange
        var request = new NameHomeRequest
        {
            HomeId = Guid.NewGuid().ToString(),
            NewName = "New Home Name"
        };
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _homeOwnerService.Setup(x => x.NameHome(_user.Id, Guid.Parse(request.HomeId), request.NewName));

        // Act
        NameHomeResponse response = _controller.NameHome(request);

        // Assert
        _homeOwnerService.Verify(x => x.NameHome(_user.Id, Guid.Parse(request.HomeId), request.NewName), Times.Once);
        response.Should().NotBeNull();
        response.HomeId.Should().Be(request.HomeId);
    }
    #endregion
}
