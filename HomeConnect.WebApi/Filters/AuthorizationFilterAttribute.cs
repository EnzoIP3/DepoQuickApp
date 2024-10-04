using System.Net;
using BusinessLogic.Users.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HomeConnect.WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizationFilterAttribute(string? permission = null) : Attribute, IAuthorizationFilter
{
    public string? Permission { get; } = permission;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userLoggedIn = context.HttpContext.Items[Item.UserLogged];
        if (userLoggedIn == null)
        {
            SetUnauthorizedResult(context, "You are not authenticated");
            return;
        }

        var user = (User)userLoggedIn;
        var requiredPermission = BuildPermission(context);
        if (!user.HasPermission(requiredPermission))
        {
            SetForbiddenResult(context, $"Missing permission: {requiredPermission}");
        }
    }

    private static string BuildPermission(AuthorizationFilterContext context)
    {
        var action = context.RouteData.Values["action"]?.ToString()?.ToLower();
        var controller = context.RouteData.Values["controller"]?.ToString()?.ToLower();
        return $"{action}-{controller}";
    }

    private static void SetUnauthorizedResult(AuthorizationFilterContext context, string message)
    {
        context.Result =
            new ObjectResult(new { InnerCode = "Unauthorized", Message = message })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
    }

    private static void SetForbiddenResult(AuthorizationFilterContext context, string message)
    {
        context.Result =
            new ObjectResult(new { InnerCode = "Forbidden", Message = message })
            {
                StatusCode = (int)HttpStatusCode.Forbidden
            };
    }
}
