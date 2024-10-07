using System.Net;
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
    public void OnAuthorization_UserNotAuthenticated_ShouldReturnUnauthorizedResult()
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
    public void OnAuthorization_WhenHomeIdIsInvalid_ShouldReturnUnauthorizedResult()
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
    public void OnAuthorization_IfUserDoesNotHavePermission_ShouldReturnsForbiddenResult()
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
        FilterTestsUtils.GetMessage(concreteResponse?.Value).Should().Be("Missing home permission: some-permission");
    }

    [TestMethod]
    public void OnAuthorization_IfHomeDoesNotExist_ShouldReturnBadRequestResult()
    {
        var homeId = Guid.NewGuid();
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _context.RouteData.Values.Add(_homeIdRoute, homeId.ToString());
        _homeOwnerServiceMock.Setup(h => h.GetHome(homeId)).Throws<ArgumentException>();
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
    public void OnAuthorization_WhenMemberIdIsInvalid_ShouldReturnBadRequestResult()
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
    public void OnAuthorization_IfMemberDoesNotExist_ShouldReturnNotFoundResult()
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
}
