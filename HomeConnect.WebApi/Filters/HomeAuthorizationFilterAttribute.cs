using System.Net;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Repositories;
using BusinessLogic.HomeOwners.Services;
using BusinessLogic.Users.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HomeConnect.WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class HomeAuthorizationFilterAttribute(IHomeOwnerService homeOwnerService, string? permission = null) : Attribute, IAuthorizationFilter
{
    private readonly string homeIdRoute = "homesId";
    public string? Permission { get; } = permission;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userLoggedIn = context.HttpContext.Items[Item.UserLogged];
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

        var homeId = context.RouteData.Values[homeIdRoute]?.ToString();
        var homeIsNotIdentified = !Guid.TryParse(homeId, out var homeIdParsed);
        if (homeIsNotIdentified)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "BadRequest",
                Message = "The home id is invalid"
            })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
            return;
        }

        var home = homeOwnerService.GetHome(homeIdParsed);
        var userLoggedMap = (User)userLoggedIn;
        var permission = BuildPermission(context);
        var member = home.Members.First(m => m.User.Id == userLoggedMap.Id);
        var hasNotPermission = !member.HasPermission(permission);

        if (hasNotPermission)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "Forbidden",
                Message = $"Missing permission: {permission.Value}"
            })
            {
                StatusCode = (int)HttpStatusCode.Forbidden
            };
        }
    }

    private HomePermission BuildPermission(AuthorizationFilterContext context)
    {
        var name = $"{context.RouteData.Values["action"].ToString().ToLower()}-{context.RouteData.Values["controller"].ToString().ToLower()}";
        return new HomePermission(name);
    }
}
