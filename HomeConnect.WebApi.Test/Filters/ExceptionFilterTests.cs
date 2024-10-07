using System.Net;
using BusinessLogic.Auth.Exceptions;
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
public class ExceptionFilterTests
{
    private readonly ExceptionFilter _attribute;
    private ExceptionContext _context = null!;

    public ExceptionFilterTests()
    {
        _attribute = new ExceptionFilter();
    }

    [TestInitialize]
    public void Initialize()
    {
        _context = new ExceptionContext(
            new ActionContext(
                new Mock<HttpContext>().Object,
                new RouteData(),
                new ActionDescriptor()),
            new List<IFilterMetadata>());
    }

    [TestMethod]
    public void OnException_WhenExceptionIsNotRegistered_ShouldResponseInternalError()
    {
        _context.Exception = new Exception("Not registered");
        _attribute.OnException(_context);

        IActionResult? response = _context.Result;

        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("InternalServerError");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should()
            .Be("There was an error when processing your request");
    }

    [TestMethod]
    public void OnException_WhenExceptionIsArgumentException_ShouldResponseBadRequest()
    {
        var exceptionMessage = "Invalid argument passed";
        _context.Exception = new ArgumentException(exceptionMessage);
        _attribute.OnException(_context);

        IActionResult? response = _context.Result;

        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("BadRequest");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should().Be(exceptionMessage);
    }

    [TestMethod]
    public void OnException_WhenExceptionIsInvalidOperationException_ShouldResponseConflict()
    {
        var exceptionMessage = "Invalid operation";
        _context.Exception = new InvalidOperationException(exceptionMessage);
        _attribute.OnException(_context);

        IActionResult? response = _context.Result;

        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.Conflict);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("Conflict");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should().Be(exceptionMessage);
    }

    [TestMethod]
    public void OnException_WhenExceptionIsAuthException_ShouldResponseUnauthorized()
    {
        var exceptionMessage = "Unauthorized";
        _context.Exception = new AuthException(exceptionMessage);
        _attribute.OnException(_context);

        IActionResult? response = _context.Result;

        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("Unauthorized");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should().Be(exceptionMessage);
    }

    [TestMethod]
    public void OnException_WhenExceptionIsKeyNotFoundException_ShouldResponseNotFound()
    {
        var exceptionMessage = "Key not found";
        _context.Exception = new KeyNotFoundException(exceptionMessage);
        _attribute.OnException(_context);

        IActionResult? response = _context.Result;

        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        FilterTestsUtils.GetInnerCode(concreteResponse.Value).Should().Be("NotFound");
        FilterTestsUtils.GetMessage(concreteResponse.Value).Should().Be(exceptionMessage);
    }
}
