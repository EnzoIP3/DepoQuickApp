using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HomeConnect.WebApi.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly Dictionary<Type, IActionResult> _errors = new Dictionary<Type, IActionResult>
    {
        {
            typeof(ArgumentException),
            new ObjectResult(new { InnerCode = "BadRequest", Message = "The request is invalid" })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            }
        },
    };

    public void OnException(ExceptionContext context)
    {
        var response = _errors.GetValueOrDefault(context.Exception.GetType());

        Console.WriteLine(context.Exception.Message);
        if (response == null)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "InternalServerError",
                Message = "There was an error when processing your request"
            })
            { StatusCode = (int)HttpStatusCode.InternalServerError };
            return;
        }

        context.Result = response;
    }
}
