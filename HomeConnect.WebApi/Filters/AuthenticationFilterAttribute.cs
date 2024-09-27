using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace HomeConnect.WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AuthenticationFilterAttribute : Attribute, IAuthorizationFilter
{
    private const string AuthorizationHeader = "Authorization";

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var authorizationHeader = context.HttpContext.Request.Headers[AuthorizationHeader];

        if (string.IsNullOrEmpty(authorizationHeader))
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "Unauthenticated",
                Message = "You are not authenticated"
            })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
            return;
        }

        var isAuthorizationFormatNotValid = !IsAuthorizationFormatValid(authorizationHeader);
        if (isAuthorizationFormatNotValid)
        {
            context.Result = new ObjectResult(
                new
                {
                    InnerCode = "InvalidAuthorization",
                    Message = "The provided authorization header format is invalid"
                })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
            return;
        }
    }

    private bool IsAuthorizationFormatValid(StringValues authorizationHeader)
    {
        if (!authorizationHeader.ToString().StartsWith("Bearer "))
        {
            return false;
        }

        var token = authorizationHeader.ToString().Substring("Bearer ".Length);

        return Guid.TryParse(token, out _);
    }
}
