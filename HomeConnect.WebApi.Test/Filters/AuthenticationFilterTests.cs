using System.Net;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Session.Repositories;
using BusinessLogic.Session.Services;
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
    private Mock<HttpContext> _httpContextMock = null!;
    private Mock<ISessionService> _sessionServiceMock = null!;
    private Mock<ISessionRepository> _authRepositoryMock = null!;
    private AuthorizationFilterContext _context = null!;
    private AuthenticationFilterAttribute _attribute = null;

    [TestInitialize]
    public void Initialize()
    {
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
        _sessionServiceMock = new Mock<ISessionService>(MockBehavior.Strict);
        _authRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);
        _attribute = new AuthenticationFilterAttribute(_authRepositoryMock.Object);

        _context = new AuthorizationFilterContext(
            new ActionContext(
                _httpContextMock.Object,
                new RouteData(),
                new ActionDescriptor()),
            new List<IFilterMetadata>());
    }

    #region Error
    [TestMethod]
    public void OnAuthorization_WhenEmptyHeaders_ShouldReturnUnauthenticatedResponse()
    {
        _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary());

        _attribute.OnAuthorization(_context);

        var response = _context.Result;

        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        FilterTestsUtils.GetInnerCode(concreteResponse?.Value).Should().Be("Unauthenticated");
        FilterTestsUtils.GetMessage(concreteResponse?.Value).Should().Be("You are not authenticated");
    }

    [TestMethod]
    public void OnAuthorization_WhenAuthorizationIsEmpty_ShouldRerturnUnauthenticatedResponse()
    {
        _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary(new Dictionary<string, StringValues>
        {
            { "Authorization", string.Empty }
        }));

        _attribute.OnAuthorization(_context);

        var response = _context.Result;

        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("Unauthenticated");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should().Be("You are not authenticated");
    }

    [TestMethod]
    public void OnAuthorization_WhenFormatIsInvalid_ShouldReturnInvalidAuthorizationResponse()
    {
        _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary(new Dictionary<string, StringValues>
        {
            { "Authorization", "Bearer 1234" }
        }));

        _attribute.OnAuthorization(_context);

        var response = _context.Result;

        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("InvalidAuthorization");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should().Be("The provided authorization header format is invalid");
    }

    [TestMethod]
    public void OnAuthorization_WhenAuthorizationExpired_ShouldReturnExpiredAuthorizationResponse()
    {
        var guid = Guid.NewGuid().ToString();
        _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary(new Dictionary<string, StringValues>
        {
            { "Authorization", $"Bearer {guid}" }
        }));
        _authRepositoryMock.Setup(a => a.IsAuthorizationExpired($"Bearer {guid}")).Returns(true);

        _attribute.OnAuthorization(_context);

        var response = _context.Result;

        _httpContextMock.VerifyAll();
        _authRepositoryMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("ExpiredAuthorization");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should().Be("The provided authorization header is expired");
    }

    [TestMethod]
    public void OnAuthorization_WhenUserDoesNotExist_ShouldReturnUnauthenticatedResponse()
    {
        var guid = Guid.NewGuid().ToString();
        _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary(new Dictionary<string, StringValues>
        {
            { "Authorization", $"Bearer {guid}" }
        }));
        _authRepositoryMock.Setup(a => a.IsAuthorizationExpired($"Bearer {guid}")).Returns(false);
        _httpContextMock.Setup(h => h.RequestServices.GetService(typeof(ISessionService))).Returns(_sessionServiceMock.Object);
        _sessionServiceMock.Setup(a => a.GetUserByToken(guid)).Returns((User?)null);
        _attribute.OnAuthorization(_context);

        var response = _context.Result;

        _httpContextMock.VerifyAll();
        _authRepositoryMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("Unauthenticated");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should().Be("You are not authenticated");
    }
    #endregion

    #region Success

    [TestMethod]
    public void OnAuthorization_WhenAuthorizationIsValid_ShouldSetUserLoggedInContext()
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
        _authRepositoryMock.Setup(a => a.IsAuthorizationExpired($"Bearer {guid}")).Returns(false);
        _sessionServiceMock.Setup(a => a.GetUserByToken(guid)).Returns(user);
        var items = new Dictionary<object, object> { { Item.UserLogged, user } };
        _httpContextMock.Setup(h => h.Items).Returns(items);
        _httpContextMock.Setup(h => h.RequestServices.GetService(typeof(ISessionService))).Returns(_sessionServiceMock.Object);

        _attribute.OnAuthorization(_context);

        _httpContextMock.VerifyAll();
        _authRepositoryMock.VerifyAll();

        _context.HttpContext.Items[Item.UserLogged].Should().NotBeNull();
        var userLogged = _context.HttpContext.Items[Item.UserLogged] as User;
        userLogged.Should().NotBeNull();
        userLogged.Should().Be(user);
    }
    #endregion
}
