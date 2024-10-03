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
            new Role { Name = "HomeOwner", Permissions = new List<SystemPermission>() });
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
        _homeOwnerService.Setup(x => x.CreateHome(args)).Returns(home.Id);

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

    #region GetMembers

    [TestMethod]
    public void GetMembers_WhenCalledWithValidRequest_ReturnsMembers()
    {
        // Arrange
        var owner = new User("owner", "owner", "email@email.com", "Password@100",
            new Role { Name = "HomeOwner", Permissions = new List<SystemPermission>() });
        var user = new User("John", "Doe", "email1@email.com", "Password@100",
            new Role { Name = "HomeOwner", Permissions = new List<SystemPermission>() });
        var otherUser = new User("Jane", "Doe", "email2@email.com", "Password@100",
            new Role { Name = "HomeOwner", Permissions = new List<SystemPermission>() });
        var home = new Home(owner, "Road 123", 123.456, 456.789, 3);
        var member = new Member(user,
            new List<HomePermission>
            {
                new HomePermission("canAddDevices"),
                new HomePermission("canListDevices"),
                new HomePermission("shouldBeNotified")
            });
        var otherMember = new Member(otherUser, new List<HomePermission> { new HomePermission("canAddDevices"), });
        home.AddMember(member);
        home.AddMember(otherMember);
        var items = new Dictionary<object, object?> { { Item.UserLogged, user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _homeOwnerService.Setup(x => x.GetHomeMembers(home.Id.ToString()))
            .Returns(new List<Member> { member, otherMember });

        var expectedResponse = new GetMembersResponse
        {
            Members = new List<ListMemberInfo>
            {
                new ListMemberInfo
                {
                    Id = member.User.Id.ToString(),
                    Name = member.User.Name,
                    Surname = member.User.Surname,
                    Photo = member.User.ProfilePhoto,
                    CanAddDevices = member.HasPermission(new HomePermission("canAddDevices")),
                    CanListDevices = member.HasPermission(new HomePermission("canListDevices")),
                    ShouldBeNotified = member.HasPermission(new HomePermission("shouldBeNotified"))
                },
                new ListMemberInfo
                {
                    Id = otherMember.User.Id.ToString(),
                    Name = otherMember.User.Name,
                    Surname = otherMember.User.Surname,
                    Photo = otherMember.User.ProfilePhoto,
                    CanAddDevices = otherMember.HasPermission(new HomePermission("canAddDevices")),
                    CanListDevices = otherMember.HasPermission(new HomePermission("canListDevices")),
                    ShouldBeNotified = otherMember.HasPermission(new HomePermission("shouldBeNotified"))
                }
            }
        };

        // Act
        var response = _controller.GetMembers(home.Id.ToString(), _context);

        // Assert
        _homeOwnerService.VerifyAll();
        response.Should().NotBeNull();
        response.Members.Should().NotBeNullOrEmpty();
        response.Members.Should().HaveCount(2);
        response.Members.Should().BeEquivalentTo(expectedResponse.Members);
    }

    #endregion
}
