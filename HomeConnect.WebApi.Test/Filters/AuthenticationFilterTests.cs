using System.Net;
using BusinessLogic.Auth.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Models;
using FluentAssertions;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;

namespace HomeConnect.WebApi.Test.Filters;

[TestClass]
public class AuthenticationFilterTests
{
    private AuthenticationFilterAttribute _attribute = null!;
    private Mock<IAuthService> _authServiceMock = null!;
    private AuthorizationFilterContext _context = null!;
    private Mock<HttpContext> _httpContextMock = null!;

    [TestInitialize]
    public void Initialize()
    {
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
        _authServiceMock = new Mock<IAuthService>(MockBehavior.Strict);
        _attribute = new AuthenticationFilterAttribute();

        _context = new AuthorizationFilterContext(
            new ActionContext(
                _httpContextMock.Object,
                new RouteData(),
                new ActionDescriptor()),
            new List<IFilterMetadata>());
    }

    #region Success

    [TestMethod]
    public void OnAuthorization_WhenAuthorizationIsValid_SetsUserLoggedInContext()
    {
        var guid = Guid.NewGuid().ToString();
        _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary(new Dictionary<string, StringValues>
        {
            { "Authorization", $"Bearer {guid}" }
        }));
        var validUserModel = new CreateUserArgs
        {
            Name = "name",
            Surname = "surname",
            Email = "email@email.com",
            Password = "Password#100",
            Role = "Admin"
        };
        var adminRole = new Role("Admin", []);
        var user = new User(validUserModel.Name, validUserModel.Surname, validUserModel.Email, validUserModel.Password,
            adminRole);
        _authServiceMock.Setup(a => a.IsTokenExpired(guid)).Returns(false);
        _authServiceMock.Setup(a => a.GetUserFromToken(guid)).Returns(user);
        var items = new Dictionary<object, object> { { Item.UserLogged, user } };
        _httpContextMock.Setup(h => h.Items).Returns(items!);
        _httpContextMock.Setup(h => h.RequestServices.GetService(typeof(IAuthService)))
            .Returns(_authServiceMock.Object);
        _authServiceMock.Setup(a => a.Exists(guid)).Returns(true);

        _attribute.OnAuthorization(_context);

        _httpContextMock.VerifyAll();
        _authServiceMock.VerifyAll();

        _context.HttpContext.Items[Item.UserLogged].Should().NotBeNull();
        var userLogged = _context.HttpContext.Items[Item.UserLogged] as User;
        userLogged.Should().NotBeNull();
        userLogged.Should().Be(user);
    }

    #endregion

    #region Error

    [TestMethod]
    public void OnAuthorization_WhenEmptyHeaders_ReturnsUnauthenticatedResponse()
    {
        _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary());

        _attribute.OnAuthorization(_context);

        IActionResult? response = _context.Result;

        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        FilterTestsUtils.GetInnerCode(concreteResponse?.Value).Should().Be("Unauthorized");
        FilterTestsUtils.GetMessage(concreteResponse?.Value).Should().Be("You are not authenticated");
    }

    [TestMethod]
    public void OnAuthorization_WhenAuthorizationIsEmpty_ReturnsUnauthenticatedResponse()
    {
        _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary(new Dictionary<string, StringValues>
        {
            { "Authorization", string.Empty }
        }));

        _attribute.OnAuthorization(_context);

        IActionResult? response = _context.Result;

        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("Unauthorized");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should().Be("You are not authenticated");
    }

    [TestMethod]
    public void OnAuthorization_WhenUnexpectedError_ReturnsInternalErrorResponse()
    {
        _httpContextMock.Setup(h => h.Request.Headers).Throws(new Exception());

        _attribute.OnAuthorization(_context);

        IActionResult? response = _context.Result;

        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("InternalError");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should()
            .Be("An error occurred while processing the request");
    }

    [TestMethod]
    public void OnAuthorization_WhenFormatIsInvalid_ReturnsInvalidAuthorizationResponse()
    {
        _httpContextMock.Setup(h => h.Request.Headers)
            .Returns(new HeaderDictionary(new Dictionary<string, StringValues> { { "Authorization", "1234" } }));

        _attribute.OnAuthorization(_context);

        IActionResult? response = _context.Result;

        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("InvalidAuthorization");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should()
            .Be("The provided authorization header format is invalid");
    }

    [TestMethod]
    public void OnAuthorization_WhenAuthorizationExpired_ReturnsExpiredAuthorizationResponse()
    {
        var guid = Guid.NewGuid().ToString();
        _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary(new Dictionary<string, StringValues>
        {
            { "Authorization", $"Bearer {guid}" }
        }));
        _httpContextMock.Setup(h => h.RequestServices.GetService(typeof(IAuthService)))
            .Returns(_authServiceMock.Object);
        _authServiceMock.Setup(a => a.Exists(guid)).Returns(true);
        _authServiceMock.Setup(a => a.IsTokenExpired(guid)).Returns(true);

        _attribute.OnAuthorization(_context);

        IActionResult? response = _context.Result;

        _httpContextMock.VerifyAll();
        _authServiceMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("ExpiredAuthorization");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should().Be("The provided authorization header is expired");
    }

    [TestMethod]
    public void OnAuthorization_WhenAuthorizationDoesNotExist_ReturnsUnauthorizedResponse()
    {
        var guid = Guid.NewGuid().ToString();
        _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary(new Dictionary<string, StringValues>
        {
            { "Authorization", $"Bearer {guid}" }
        }));
        _httpContextMock.Setup(h => h.RequestServices.GetService(typeof(IAuthService)))
            .Returns(_authServiceMock.Object);
        _authServiceMock.Setup(a => a.Exists(guid)).Returns(false);

        _attribute.OnAuthorization(_context);

        IActionResult? response = _context.Result;

        _httpContextMock.VerifyAll();
        _authServiceMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("Unauthorized");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should().Be("The provided authorization header is invalid");
    }

    #endregion
}
