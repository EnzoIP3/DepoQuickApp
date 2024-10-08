using System.Net;
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
public class AuthorizationFilterTests
{
    private AuthorizationFilterAttribute _attribute = null!;
    private AuthorizationFilterContext _context = null!;
    private Mock<HttpContext> _httpContextMock = null!;
    private Mock<IUserRepository> _userRepositoryMock = null!;

    [TestInitialize]
    public void Initialize()
    {
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
        _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        _attribute = new AuthorizationFilterAttribute("some-permission");

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
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        FilterTestsUtils.GetInnerCode(concreteResponse?.Value).Should().Be("Unauthorized");
        FilterTestsUtils.GetMessage(concreteResponse?.Value).Should().Be("You are not authenticated");
    }

    [TestMethod]
    public void OnAuthorization_WhenUserDoesNotHavePermission_ReturnsForbiddenResult()
    {
        var items = new Dictionary<object, object?>
        {
            {
                Item.UserLogged, new User("Name", "Surname", "email@email.com", "Password@100",
                    new Role("Admin", []))
            }
        };
        _httpContextMock.Setup(h => h.Items).Returns(items);

        _attribute.OnAuthorization(_context);

        IActionResult? response = _context.Result;

        _httpContextMock.VerifyAll();
        _userRepositoryMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        FilterTestsUtils.GetInnerCode(concreteResponse?.Value).Should().Be("Forbidden");
        FilterTestsUtils.GetMessage(concreteResponse?.Value).Should().Be("Missing permission: some-permission");
    }
}
