using System.Net;
using BusinessLogic;
using FluentAssertions;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace HomeConnect.WebApi.Test;

[TestClass]
public class AuthorizationFilterTests
{
    private Mock<HttpContext> _httpContextMock = null!;
    private Mock<IUserRepository> _userRepositoryMock = null!;
    private AuthorizationFilterContext _context = null!;
    private AuthorizationFilterAttribute _attribute = null;

    [TestInitialize]
    public void Initialize()
    {
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
        _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        _attribute = new AuthorizationFilterAttribute(_userRepositoryMock.Object, "some-permission");

        _context = new AuthorizationFilterContext(
            new ActionContext(
                _httpContextMock.Object,
                new RouteData(),
                new ActionDescriptor()
            ),
            new List<IFilterMetadata>()
        );
    }

    [TestMethod]
    public void OnAuthorization_UserNotAuthenticated_ShouldReturnsUnauthorizedResult()
    {
        var items = new Dictionary<object, object?>
        {
            { Items.UserLogged, null }
        };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _attribute.OnAuthorization(_context);

        var response = _context.Result;

        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        GetInnerCode(concreteResponse?.Value).Should().Be("Unauthorized");
        GetMessage(concreteResponse?.Value).Should().Be("You are not authenticated");
    }

    [TestMethod]
    public void OnAuthorization_IfUserDoesNotHavePermission_ShouldReturnsForbiddenResult()
    {
        var items = new Dictionary<object, object?>
        {
            {
                Items.UserLogged,
                new UserModel
                {
                    Name = "Name",
                    Surname = "Surname",
                    Email = "email@email.com",
                    Password = "Password@100",
                    Role = "Admin"
                }
            }
        };
        var role = new Role("Admin", new List<SystemPermission> { new SystemPermission("some-permission") });

        _httpContextMock.Setup(h => h.Items).Returns(items);

        // Mock RouteData
        var routeData = new RouteData();
        routeData.Values["action"] = "SomeAction";
        routeData.Values["controller"] = "SomeController";
        var actionContext = new ActionContext(_httpContextMock.Object, routeData, new ActionDescriptor());
        _context = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());

        _userRepositoryMock.Setup(u => u.GetUser("email@email.com"))
            .Returns(new User("Name", "Surname", "email@email.com", "Password@100", role));
        _attribute.OnAuthorization(_context);

        var response = _context.Result;

        _httpContextMock.VerifyAll();
        _userRepositoryMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        GetInnerCode(concreteResponse?.Value).Should().Be("Forbidden");
        GetMessage(concreteResponse?.Value).Should().Be("Missing permission: someaction-somecontroller");
    }

    private string GetInnerCode(object? value)
    {
        return value?.GetType().GetProperty("InnerCode")?.GetValue(value)?.ToString() ?? string.Empty;
    }

    private string GetMessage(object? value)
    {
        return value?.GetType().GetProperty("Message")?.GetValue(value)?.ToString() ?? string.Empty;
    }
}
