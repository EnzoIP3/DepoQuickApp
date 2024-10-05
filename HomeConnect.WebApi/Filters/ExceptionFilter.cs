using System.Net;
using BusinessLogic.Auth.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HomeConnect.WebApi.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly Dictionary<Type, Func<Exception, IActionResult>> _errors =
        new Dictionary<Type, Func<Exception, IActionResult>>
        {
            {
                typeof(ArgumentException),
                ex => new ObjectResult(new { InnerCode = "BadRequest", ex.Message })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                }
            },
            {
                typeof(InvalidOperationException),
                ex => new ObjectResult(new { InnerCode = "Conflict", ex.Message })
                {
                    StatusCode = (int)HttpStatusCode.Conflict
                }
            },
            {
                typeof(AuthException),
                ex => new ObjectResult(new { InnerCode = "Unauthorized", ex.Message })
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                }
            }
        };

    public void OnException(ExceptionContext context)
    {
        var exceptionType = context.Exception.GetType();
        var exceptionMessage = context.Exception.Message;

        var response = _errors.GetValueOrDefault(exceptionType)?.Invoke(context.Exception);

        if (response == null)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "InternalServerError", Message = "There was an error when processing your request"
            }) { StatusCode = (int)HttpStatusCode.InternalServerError };
        }
        else
        {
            context.Result = response;
        }

        Console.WriteLine(exceptionMessage);
    }
}
