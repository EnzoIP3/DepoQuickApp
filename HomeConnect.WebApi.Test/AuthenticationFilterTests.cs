using System.Diagnostics;
using System.Net;
using FluentAssertions;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;

namespace HomeConnect.WebApi.Test;

[TestClass]
public class AuthenticationFilterTests
{
    private Mock<HttpContext> _httpContextMock = null!;
    private AuthorizationFilterContext _context = null!;
    private AuthenticationFilterAttribute _attribute = new();

    [TestInitialize]
    public void Initialize()
    {
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);

        _context = new AuthorizationFilterContext(
            new ActionContext(
                _httpContextMock.Object,
                new RouteData(),
                new ActionDescriptor()
            ),
            new List<IFilterMetadata>()
        );
    }

    #region Error
    [TestMethod]
    public void OnAuthorization_WhenEmptyHeaders_ShouldRerturnUnauthenticatedResponse()
    {
        _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary());

        _attribute.OnAuthorization(_context);

        var response = _context.Result;

        _httpContextMock.VerifyAll();
        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        GetInnerCode(concreteResponse?.Value).Should().Be("Unauthenticated");
        GetMessage(concreteResponse?.Value).Should().Be("You are not authenticated");
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
        GetInnerCode(concreteResponse.Value).Should().Be("Unauthenticated");
        GetMessage(concreteResponse.Value).Should().Be("You are not authenticated");
    }
    #endregion

    private string GetInnerCode(object? value)
    {
        return value?.GetType().GetProperty("InnerCode")?.GetValue(value)?.ToString() ?? string.Empty;
    }

    private string GetMessage(object? value)
    {
        return value?.GetType().GetProperty("Message")?.GetValue(value)?.ToString() ?? string.Empty;
    }
}
