using System.Net;
using BusinessLogic;
using HomeConnect.WebApi.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace HomeConnect.WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AuthenticationFilterAttribute(IAuthRepository authRepository) : Attribute, IAuthorizationFilter
{
    public IAuthRepository AuthRepository { get; } = authRepository;
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

        var isAuthorizationExpired = IsAuthorizationExpired(authorizationHeader);
        if (isAuthorizationExpired)
        {
            context.Result = new ObjectResult(
                new
                {
                    InnerCode = "ExpiredAuthorization",
                    Message = "The provided authorization header is expired"
                })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
            return;
        }

        try
        {
            var userOfAuthorization = GetUserOfAuthorization(authorizationHeader, context);

            if (userOfAuthorization == null)
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

            context.HttpContext.Items[Item.UserLogged] = userOfAuthorization;
        }
        catch (Exception)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "InternalError",
                Message = "An error ocurred while processing the request"
            })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }

    private User? GetUserOfAuthorization(
        StringValues authorization,
        AuthorizationFilterContext context)
    {
        var token = authorization.ToString().Substring("Bearer ".Length);
        var sessionService = context.HttpContext.RequestServices.GetRequiredService<ISessionService>();
        var user = sessionService.GetUserByToken(token);

        return user;
    }

    private bool IsAuthorizationExpired(StringValues authorizationHeader)
    {
        return AuthRepository.IsAuthorizationExpired(authorizationHeader!);
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
