using System.Net;
using BusinessLogic.Users.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HomeConnect.WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizationFilterAttribute(string permission) : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userLoggedIn = context.HttpContext.Items[Item.UserLogged];
        if (userLoggedIn == null)
        {
            SetUnauthorizedResult(context, "You are not authenticated");
            return;
        }

        var user = (User)userLoggedIn;
        if (!user.HasPermission(permission))
        {
            SetForbiddenResult(context, $"Missing permission: {permission}");
        }
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
