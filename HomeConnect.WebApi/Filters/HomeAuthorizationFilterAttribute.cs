using System.Net;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Repositories;
using BusinessLogic.Users.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HomeConnect.WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class HomeAuthorizationFilterAttribute(string? permission = null) : Attribute, IAuthorizationFilter
{
    private string homeIdRoute = "homesId";
    public string? Permission { get; } = permission;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userLoggedIn = context.HttpContext.Items[Item.UserLogged];
        var userIsNotIdentified = userLoggedIn == null;

        if (userIsNotIdentified)
        {
            context.Result =
                new ObjectResult(new { InnerCode = "Unauthorized", Message = "You are not authenticated" })
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
            return;
        }

        var homeId = context.RouteData.Values[homeIdRoute]?.ToString();
        var homeIsNotIdentified = !Guid.TryParse(homeId, out var home);
        if (homeIsNotIdentified)
        {
            context.Result =
                new ObjectResult(new { InnerCode = "BadRequest", Message = "The home id is invalid" })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            return;
        }
    }
}
