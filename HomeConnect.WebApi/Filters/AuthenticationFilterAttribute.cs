using System.Net;
using BusinessLogic.Sessions.Services;
using BusinessLogic.Users.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace HomeConnect.WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AuthenticationFilterAttribute : Attribute, IAuthorizationFilter
{
    private const string AuthorizationHeader = "Authorization";
    private const string BearerPrefix = "Bearer ";

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        try
        {
            var authorizationHeader = GetAuthorizationHeader(context);

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                SetUnauthorizedResult(context, "Unauthenticated", "You are not authenticated");
                return;
            }

            if (!IsAuthorizationFormatValid(authorizationHeader))
            {
                SetUnauthorizedResult(context, "InvalidAuthorization",
                    "The provided authorization header format is invalid");
                return;
            }

            if (IsSessionExpired(authorizationHeader, context))
            {
                SetUnauthorizedResult(context, "ExpiredAuthorization", "The provided authorization header is expired");
                return;
            }

            var user = GetUserOfAuthorization(authorizationHeader, context);

            if (user == null)
            {
                SetUnauthorizedResult(context, "Unauthenticated", "You are not authenticated");
                return;
            }

            context.HttpContext.Items[Item.UserLogged] = user;
        }
        catch (Exception)
        {
            SetInternalServerErrorResult(context);
        }
    }

    private StringValues GetAuthorizationHeader(AuthorizationFilterContext context)
    {
        return context.HttpContext.Request.Headers[AuthorizationHeader];
    }

    private void SetUnauthorizedResult(AuthorizationFilterContext context, string innerCode, string message)
    {
        context.Result = new ObjectResult(new { InnerCode = innerCode, Message = message })
        {
            StatusCode = (int)HttpStatusCode.Unauthorized
        };
    }

    private void SetInternalServerErrorResult(AuthorizationFilterContext context)
    {
        context.Result = new ObjectResult(new
        {
            InnerCode = "InternalError", Message = "An error occurred while processing the request"
        }) { StatusCode = (int)HttpStatusCode.InternalServerError };
    }

    private User GetUserOfAuthorization(StringValues authorization, AuthorizationFilterContext context)
    {
        var sessionId = ExtractSessionId(authorization);
        var sessionService = GetSessionService(context);
        return sessionService.GetUserFromSession(sessionId);
    }

    private bool IsSessionExpired(StringValues authorizationHeader, AuthorizationFilterContext context)
    {
        var sessionService = GetSessionService(context);
        var sessionId = ExtractSessionId(authorizationHeader);
        return sessionService.IsSessionExpired(sessionId);
    }

    private bool IsAuthorizationFormatValid(StringValues authorizationHeader)
    {
        if (!authorizationHeader.ToString().StartsWith(BearerPrefix))
        {
            return false;
        }

        var sessionId = ExtractSessionId(authorizationHeader);
        return Guid.TryParse(sessionId, out _);
    }

    private string ExtractSessionId(StringValues authorizationHeader)
    {
        return authorizationHeader.ToString().Substring(BearerPrefix.Length);
    }

    private ISessionService GetSessionService(AuthorizationFilterContext context)
    {
        return context.HttpContext.RequestServices.GetRequiredService<ISessionService>();
    }
}
