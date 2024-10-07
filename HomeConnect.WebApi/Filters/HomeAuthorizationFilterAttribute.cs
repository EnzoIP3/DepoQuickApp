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
        var homeIsNotIdentified = !Guid.TryParse(homeId, out Guid homeIdParsed);
        if (homeIsNotIdentified)
        {
            context.Result =
                new ObjectResult(new { InnerCode = "BadRequest", Message = "The home id is invalid" })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            return;
        }

        Home? home = GetHomeById(context, homeIdParsed);
        if (home == null)
        {
            context.Result =
                new ObjectResult(new { InnerCode = "BadRequest", Message = "The home does not exist" })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            return;
        }

        var userLoggedMap = (User)userLoggedIn!;
        var homePermission = new HomePermission(permission);
        var hasPermission = home.Owner.Id == userLoggedMap.Id;
        Console.WriteLine(home.Owner.Id);
        Console.WriteLine(userLoggedMap.Id);

        if (!hasPermission)
        {
            Member? member = home.Members.FirstOrDefault(m => m.User.Id == userLoggedMap.Id);
            hasPermission = member != null && member.HasPermission(homePermission);
        }

        if (!hasPermission)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "Forbidden", Message = $"Missing home permission: {permission}"
            }) { StatusCode = (int)HttpStatusCode.Forbidden };
        }
    }

    private static Home? GetHomeById(AuthorizationFilterContext context, Guid homeIdParsed)
    {
        IHomeOwnerService homeOwnerService = GetHomeOwnerService(context);
        Home? home;
        try
        {
            home = homeOwnerService.GetHome(homeIdParsed);
        }
        catch (ArgumentException)
        {
            return null;
        }

        return home;
    }

    private static IHomeOwnerService GetHomeOwnerService(AuthorizationFilterContext context)
    {
        return context.HttpContext.RequestServices.GetRequiredService<IHomeOwnerService>();
    }
}
