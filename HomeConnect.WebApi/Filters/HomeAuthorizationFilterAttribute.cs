using System.Net;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Services;
using BusinessLogic.Users.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HomeConnect.WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class HomeAuthorizationFilterAttribute(string permission) : Attribute, IAuthorizationFilter
{
    private const string HomeIdRoute = "homesId";

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

        var homeId = context.RouteData.Values[HomeIdRoute]?.ToString();
        var homeIsNotIdentified = !Guid.TryParse(homeId, out var homeIdParsed);
        if (homeIsNotIdentified)
        {
            context.Result =
                new ObjectResult(new { InnerCode = "BadRequest", Message = "The home id is invalid" })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            return;
        }

        var homeOwnerService = GetHomeOwnerService(context);
        var home = homeOwnerService.GetHome(homeIdParsed);
        var userLoggedMap = (User)userLoggedIn;
        var homePermission = new HomePermission(permission);
        var member = home.Members.First(m => m.User.Id == userLoggedMap.Id);
        var hasNotPermission = !member.HasPermission(homePermission);

        if (hasNotPermission)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "Forbidden",
                Message = $"Missing permission: {permission}"
            })
            { StatusCode = (int)HttpStatusCode.Forbidden };
        }
    }

    private static IHomeOwnerService GetHomeOwnerService(AuthorizationFilterContext context)
    {
        return context.HttpContext.RequestServices.GetRequiredService<IHomeOwnerService>();
    }
}
