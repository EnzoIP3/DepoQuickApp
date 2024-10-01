using System.Net;
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
    private ExceptionContext _context = null!;
    private readonly ExceptionFilter _attribute;

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

        var response = _context.Result;

        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        FilterTestsUtils.GetInnerCode(concreteResponse?.Value).Should().Be("InternalServerError");
        FilterTestsUtils.GetMessage(concreteResponse?.Value).Should().Be("There was an error when processing your request");
    }

    [TestMethod]
    public void OnException_WhenExceptionIsArgumentException_ShouldResponseBadRequest()
    {
        _context.Exception = new ArgumentException("Not registered");
        _attribute.OnException(_context);

        var response = _context.Result;

        response.Should().NotBeNull();
        var concreteResponse = response as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        FilterTestsUtils.GetInnerCode(concreteResponse?.Value).Should().Be("BadRequest");
        FilterTestsUtils.GetMessage(concreteResponse?.Value).Should().Be("The request is invalid");
    }
}
