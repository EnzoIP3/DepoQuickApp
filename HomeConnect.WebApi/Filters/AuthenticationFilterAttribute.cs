using System.Net;
using BusinessLogic.Auth.Services;
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
            StringValues authorizationHeader = GetAuthorizationHeader(context);

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                SetUnauthorizedResult(context, "Unauthorized", "You are not authenticated");
                return;
            }

            if (!IsAuthorizationFormatValid(authorizationHeader))
            {
                SetUnauthorizedResult(context, "InvalidAuthorization",
                    "The provided authorization header format is invalid");
                return;
            }

            if (!AuthorizationExists(context, authorizationHeader))
            {
                SetUnauthorizedResult(context, "Unauthorized", "The provided authorization header is expired");
                return;
            }

            if (IsTokenExpired(authorizationHeader, context))
            {
                SetUnauthorizedResult(context, "ExpiredAuthorization", "The provided authorization header is expired");
                return;
            }

            User user = GetUserOfAuthorization(authorizationHeader, context);

            context.HttpContext.Items[Item.UserLogged] = user;
        }
        catch (Exception)
        {
            SetInternalServerErrorResult(context);
        }
    }

    private bool AuthorizationExists(AuthorizationFilterContext context, StringValues authorizationHeader)
    {
        IAuthService tokenService = GetTokenService(context);
        return tokenService.Exists(ExtractTokenFromAuthorization(authorizationHeader));
    }

    private static StringValues GetAuthorizationHeader(AuthorizationFilterContext context)
    {
        return context.HttpContext.Request.Headers[AuthorizationHeader];
    }

    private static void SetUnauthorizedResult(AuthorizationFilterContext context, string innerCode, string message)
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

    private static User GetUserOfAuthorization(StringValues authorization, AuthorizationFilterContext context)
    {
        var token = ExtractTokenFromAuthorization(authorization);
        IAuthService tokenService = GetTokenService(context);
        return tokenService.GetUserFromToken(token);
    }

    private static bool IsTokenExpired(StringValues authorizationHeader, AuthorizationFilterContext context)
    {
        IAuthService tokenService = GetTokenService(context);
        var token = ExtractTokenFromAuthorization(authorizationHeader);
        return tokenService.IsTokenExpired(token);
    }

    private bool IsAuthorizationFormatValid(StringValues authorizationHeader)
    {
        if (!authorizationHeader.ToString().StartsWith(BearerPrefix))
        {
            return false;
        }

        var token = ExtractTokenFromAuthorization(authorizationHeader);
        return Guid.TryParse(token, out _);
    }

    private static string ExtractTokenFromAuthorization(StringValues authorizationHeader)
    {
        return authorizationHeader.ToString().Substring(BearerPrefix.Length);
    }

    private static IAuthService GetTokenService(AuthorizationFilterContext context)
    {
        return context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
    }
}
