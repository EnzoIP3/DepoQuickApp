using System.Net;
using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Repositories;
using FluentAssertions;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace HomeConnect.WebApi.Test.Filters;

[TestClass]
public class HomeAuthorizationFilterAttributeTests
{
    private readonly string _homeIdRoute = "homesId";

    private readonly User _user = new("name", "surname", "email@email.com", "Password@100",
        new Role { Name = "HomeOwner", Permissions = [] });

    private HomeAuthorizationFilterAttribute _attribute = null!;
    private AuthorizationFilterContext _context = null!;
    private Mock<IHomeOwnerService> _homeOwnerServiceMock = null!;
    private Mock<HttpContext> _httpContextMock = null!;
    private Mock<IUserRepository> _userRepositoryMock = null!;

    [TestInitialize]
    public void Initialize()
    {
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
        _homeOwnerServiceMock = new Mock<IHomeOwnerService>(MockBehavior.Strict);
        _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        _attribute = new HomeAuthorizationFilterAttribute("some-permission");

        _context = new AuthorizationFilterContext(
            new ActionContext(
                _httpContextMock.Object,
                new RouteData(),
                new ActionDescriptor()),
            new List<IFilterMetadata>());
    }

    [TestMethod]
    public void OnAuthorization_WhenUserNotAuthenticated_ReturnsUnauthorizedResult()
    {
        var items = new Dictionary<object, object?> { { Item.UserLogged, null } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _attribute.OnAuthorization(_context);

        IActionResult? response = _context.Result;

        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("Unauthorized");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should().Be("You are not authenticated");
    }

    [TestMethod]
    public void OnAuthorization_WhenHomeIdIsInvalid_ReturnsUnauthorizedResult()
    {
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _context.RouteData.Values.Add("homesId", " ");
        _attribute.OnAuthorization(_context);

        IActionResult? response = _context.Result;

        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("BadRequest");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should().Be("The home ID is invalid");
    }

    [TestMethod]
    public void OnAuthorization_WhenUserDoesNotHavePermission_ReturnsForbiddenResult()
    {
        var otherUser = new User("Name2", "Surname2", "email2@email.com", "Password@100",
            new Role { Name = "HomeOwner", Permissions = [] });
        var home = new Home(otherUser, "street 123", 50.456, 123.456, 2);
        home.AddMember(new Member(_user));
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _homeOwnerServiceMock.Setup(h => h.GetHome(home.Id)).Returns(home);
        _httpContextMock.Setup(h => h.RequestServices.GetService(typeof(IHomeOwnerService)))
            .Returns(_homeOwnerServiceMock.Object);

        var routeData = new RouteData();
        routeData.Values[_homeIdRoute] = home.Id.ToString();
        _context.RouteData = routeData;

        _attribute.OnAuthorization(_context);

        IActionResult? response = _context.Result;

        _httpContextMock.VerifyAll();
        _userRepositoryMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        FilterTestsUtils.GetInnerCode(concreteResponse?.Value).Should().Be("Forbidden");
        FilterTestsUtils.GetMessage(concreteResponse?.Value).Should().Be("You are not allowed to do that in this home");
    }

    [TestMethod]
    public void OnAuthorization_WhenHomeDoesNotExist_ReturnsBadRequestResult()
    {
        var homeId = Guid.NewGuid();
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _context.RouteData.Values.Add(_homeIdRoute, homeId.ToString());
        _homeOwnerServiceMock.Setup(h => h.GetHome(homeId)).Throws<KeyNotFoundException>();
        _httpContextMock.Setup(h => h.RequestServices.GetService(typeof(IHomeOwnerService)))
            .Returns(_homeOwnerServiceMock.Object);

        _attribute.OnAuthorization(_context);

        IActionResult? response = _context.Result;

        _httpContextMock.VerifyAll();
        _userRepositoryMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("NotFound");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should().Be("The home does not exist");
    }

    [TestMethod]
    public void OnAuthorization_WhenMemberIdIsInvalid_ReturnsBadRequestResult()
    {
        // Arrange
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _context.RouteData.Values.Add("membersId", " ");

        // Act
        _attribute.OnAuthorization(_context);

        // Assert
        IActionResult? response = _context.Result;
        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("BadRequest");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should().Be("The member ID is invalid");
    }

    [TestMethod]
    public void OnAuthorization_WhenMemberDoesNotExist_ReturnsNotFoundResult()
    {
        // Arrange
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        var memberId = Guid.NewGuid();
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _context.RouteData.Values.Add("membersId", memberId.ToString());

        _homeOwnerServiceMock.Setup(h => h.GetMemberById(memberId))
            .Throws<ArgumentException>();
        _httpContextMock.Setup(h => h.RequestServices.GetService(typeof(IHomeOwnerService)))
            .Returns(_homeOwnerServiceMock.Object);

        // Act
        _attribute.OnAuthorization(_context);

        // Assert
        IActionResult? response = _context.Result;
        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("NotFound");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should().Be("The member does not exist");
    }

    [TestMethod]
    public void OnAuthorization_WhenUserIsMemberButLacksPermission_ReturnsForbiddenResult()
    {
        // Arrange
        var owner = new User();
        var home = new Home(owner, "street 123", 50.456, 123.456, 2);
        var member = new Member(_user) { Home = home };

        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _context.RouteData.Values.Add("membersId", member.Id.ToString());

        _homeOwnerServiceMock.Setup(h => h.GetMemberById(member.Id)).Returns(member);
        _httpContextMock.Setup(h => h.RequestServices.GetService(typeof(IHomeOwnerService)))
            .Returns(_homeOwnerServiceMock.Object);

        // Act
        _attribute.OnAuthorization(_context);

        // Assert
        IActionResult? response = _context.Result;
        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("Forbidden");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should().Be("You are not allowed to do that in this home");
    }

    [TestMethod]
    public void OnAuthorization_WhenUserIsNotMemberAndPermissionIsEmpty_ReturnsForbiddenResult()
    {
        // Arrange
        var owner = new User();
        var home = new Home(owner, "street 123", 50.456, 123.456, 2);
        var member = new Member(_user) { Home = home };

        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _context.RouteData.Values.Add("membersId", member.Id.ToString());

        _homeOwnerServiceMock.Setup(h => h.GetMemberById(member.Id)).Returns(member);
        _httpContextMock.Setup(h => h.RequestServices.GetService(typeof(IHomeOwnerService)))
            .Returns(_homeOwnerServiceMock.Object);

        // Act
        _attribute = new HomeAuthorizationFilterAttribute();
        _attribute.OnAuthorization(_context);

        // Assert
        IActionResult? response = _context.Result;
        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("Forbidden");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should().Be("You do not belong to this home");
    }

    [TestMethod]
    public void OnAuthorization_WhenHardwareIdIsInvalid_ReturnsBadRequestResult()
    {
        // Arrange
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _context.RouteData.Values.Add("hardwareId", " ");

        // Act
        _attribute.OnAuthorization(_context);

        // Assert
        IActionResult? response = _context.Result;
        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("BadRequest");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should().Be("The hardware ID is invalid");
    }

    [TestMethod]
    public void OnAuthorization_WhenDeviceDoesNotExist_ReturnsNotFoundResult()
    {
        // Arrange
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        var hardwareId = Guid.NewGuid().ToString();
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _context.RouteData.Values.Add("hardwareId", hardwareId);

        _homeOwnerServiceMock.Setup(h => h.GetOwnedDeviceByHardwareId(hardwareId))
            .Throws<KeyNotFoundException>();
        _httpContextMock.Setup(h => h.RequestServices.GetService(typeof(IHomeOwnerService)))
            .Returns(_homeOwnerServiceMock.Object);

        // Act
        _attribute.OnAuthorization(_context);

        // Assert
        IActionResult? response = _context.Result;
        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("NotFound");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should().Be("The device does not exist");
    }

    [TestMethod]
    public void OnAuthorization_WhenDeviceExistsButUserDoesNotHavePermission_ReturnsForbiddenResult()
    {
        // Arrange
        var otherUser = new User("Name2", "Surname2", "email2@email.com", "Password@100",
            new Role { Name = "HomeOwner", Permissions = [] });
        var home = new Home(otherUser, "street 123", 50.456, 123.456, 2);
        home.AddMember(new Member(_user));
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        var hardwareId = Guid.NewGuid().ToString();
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _context.RouteData.Values.Add("hardwareId", hardwareId);

        _homeOwnerServiceMock.Setup(h => h.GetOwnedDeviceByHardwareId(hardwareId))
            .Returns(new OwnedDevice { Home = home });
        _httpContextMock.Setup(h => h.RequestServices.GetService(typeof(IHomeOwnerService)))
            .Returns(_homeOwnerServiceMock.Object);

        // Act
        _attribute.OnAuthorization(_context);

        // Assert
        IActionResult? response = _context.Result;
        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("Forbidden");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should().Be("You are not allowed to do that in this home");
    }

    [TestMethod]
    public void OnAuthorization_WhenRoomIdIsInvalid_ReturnsBadRequestResult()
    {
        // Arrange
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _context.RouteData.Values.Add("roomId", " ");

        // Act
        _attribute.OnAuthorization(_context);

        // Assert
        IActionResult? response = _context.Result;
        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("BadRequest");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should().Be("The room ID is invalid");
    }

    [TestMethod]
    public void OnAuthorization_WhenRoomDoesNotExist_ReturnsNotFoundResult()
    {
        // Arrange
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        var roomId = Guid.NewGuid();
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _context.RouteData.Values.Add("roomId", roomId.ToString());

        _homeOwnerServiceMock.Setup(h => h.GetRoom(roomId.ToString()))
            .Throws<KeyNotFoundException>();
        _httpContextMock.Setup(h => h.RequestServices.GetService(typeof(IHomeOwnerService)))
            .Returns(_homeOwnerServiceMock.Object);

        // Act
        _attribute.OnAuthorization(_context);

        // Assert
        IActionResult? response = _context.Result;
        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("NotFound");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should().Be("The room does not exist");
    }

    [TestMethod]
    public void OnAuthorization_WhenRoomExistsButUserDoesNotHavePermission_ReturnsForbiddenResult()
    {
        // Arrange
        var otherUser = new User("Name2", "Surname2", "email2@email.com", "Password@100",
            new Role { Name = "HomeOwner", Permissions = [] });
        var home = new Home(otherUser, "street 123", 50.456, 123.456, 2);
        home.AddMember(new Member(_user));
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        var roomId = Guid.NewGuid();
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _httpContextMock.Setup(h => h.RequestServices.GetService(typeof(IHomeOwnerService)))
            .Returns(_homeOwnerServiceMock.Object);
        _homeOwnerServiceMock.Setup(h => h.GetRoom(roomId.ToString())).Returns(new Room { Home = home });
        _context.RouteData.Values.Add("roomId", roomId.ToString());

        // Act
        _attribute.OnAuthorization(_context);

        // Assert
        IActionResult? response = _context.Result;
        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("Forbidden");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should().Be("You are not allowed to do that in this home");
    }
}
