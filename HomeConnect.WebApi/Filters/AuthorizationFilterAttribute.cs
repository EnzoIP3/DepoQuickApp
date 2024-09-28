using System.Net;
using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HomeConnect.WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizationFilterAttribute(string? permission = null) : Attribute, IAuthorizationFilter
{
    public string? Permission { get; } = permission;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userLoggedIn = context.HttpContext.Items[Items.UserLogged];
        var userIsNotIdentified = userLoggedIn == null;

        if (userIsNotIdentified)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "Unauthorized",
                Message = "You are not authenticated"
            })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
            return;
        }

        var userLoggedMap = (User)userLoggedIn;
        var permission = BuildPermission(context);
        var hasNotPermission = !userLoggedMap.HasPermission(permission);

        if (hasNotPermission)
        {
               context.Result = new ObjectResult(new
                {
                    InnerCode = "Forbidden",
                    Message = $"Missing permission: {permission}"
                })
                {
                    StatusCode = (int)HttpStatusCode.Forbidden
                };
        }
    }

    private string BuildPermission(AuthorizationFilterContext context)
    {
        return $"{context.RouteData.Values["action"].ToString().ToLower()}-{context.RouteData.Values["controller"].ToString().ToLower()}";
    }
}
