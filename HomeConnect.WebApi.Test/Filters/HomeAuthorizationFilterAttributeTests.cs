using System.Net;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Repositories;
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
    private Mock<HttpContext> _httpContextMock = null!;
    private Mock<IUserRepository> _userRepositoryMock = null!;
    private Mock<IHomeRepository> _homeRepositoryMock = null!;
    private AuthorizationFilterContext _context = null!;
    private HomeAuthorizationFilterAttribute _attribute = null;
    private readonly string _homeIdRoute = "homesId";
    private readonly User _user = new User("name", "surname", "email@email.com", "Password@100",
        new Role { Name = "HomeOwner", Permissions = [] });

    [TestInitialize]
    public void Initialize()
    {
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
        _homeRepositoryMock = new Mock<IHomeRepository>(MockBehavior.Strict);
        _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        _attribute = new HomeAuthorizationFilterAttribute(_homeRepositoryMock.Object, "some-permission");

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

        var response = _context.Result;

        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        FilterTestsUtils.GetInnerCode(concreteResponse?.Value).Should().Be("Unauthorized");
        FilterTestsUtils.GetMessage(concreteResponse?.Value).Should().Be("You are not authenticated");
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [DataRow(" ")]
    public void OnAuthorization_WhenHomeIdIsInvalid_ShouldReturnUnauthorizedResult(string homeId)
    {
        var items = new Dictionary<object, object?> { { Item.UserLogged, _user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _context.RouteData.Values.Add("homesId", homeId);
        _attribute.OnAuthorization(_context);

        var response = _context.Result;

        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        FilterTestsUtils.GetInnerCode(concreteResponse?.Value).Should().Be("BadRequest");
        FilterTestsUtils.GetMessage(concreteResponse?.Value).Should().Be("The home id is invalid");
    }

    [TestMethod]
    public void OnAuthorization_IfUserDoesNotHavePermission_ShouldReturnsForbiddenResult()
    {
        var otherUser = new User("Name2", "Surname2", "email2@email.com", "Password@100",
            new Role { Name = "HomeOwner", Permissions = [] });
        var home = new Home(otherUser, "street 123", 123.456, 123.456, 2);
        home.AddMember(new Member(_user));
        var items = new Dictionary<object, object?>
        {
            {
                Item.UserLogged,
                _user
            }
        };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _homeRepositoryMock.Setup(h => h.Get(home.Id)).Returns(home);

        var routeData = new RouteData();
        routeData.Values["action"] = "SomeAction";
        routeData.Values["controller"] = "SomeController";
        routeData.Values[_homeIdRoute] = home.Id.ToString();
        _context.RouteData = routeData;

        _attribute.OnAuthorization(_context);

        var response = _context.Result;

        _httpContextMock.VerifyAll();
        _userRepositoryMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        FilterTestsUtils.GetInnerCode(concreteResponse?.Value).Should().Be("Forbidden");
        FilterTestsUtils.GetMessage(concreteResponse?.Value).Should().Be("Missing permission: someaction-somecontroller");
    }
}
