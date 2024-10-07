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
    private const string MemberIdRoute = "membersId";

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = GetAuthenticatedUser(context);
        if (user == null)
        {
            SetUnauthorizedResult(context);
            return;
        }

        var homeId = GetRouteValueAsString(context, HomeIdRoute);
        var memberId = GetRouteValueAsString(context, MemberIdRoute);

        if (homeId != null)
        {
            if (!IsValidGuid(homeId, out var homeIdParsed))
            {
                SetBadRequestResult(context, "The home ID is invalid");
                return;
            }

            var home = GetHomeById(context, homeIdParsed);
            if (home == null)
            {
                SetNotFoundResult(context, "The home does not exist");
                return;
            }

            if (!UserHasRequiredPermission(user, home, permission))
            {
                SetForbiddenResult(context, permission);
            }

            return;
        }

        if (memberId != null)
        {
            if (!IsValidGuid(memberId, out var memberIdParsed))
            {
                SetBadRequestResult(context, "The member ID is invalid");
                return;
            }

            var home = GetHomeFromMember(context, memberIdParsed);
            if (home == null)
            {
                SetNotFoundResult(context, "The member does not exist");
                return;
            }

            if (!UserHasRequiredPermission(user, home, permission))
            {
                SetForbiddenResult(context, permission);
            }
        }
    }

    private static User? GetAuthenticatedUser(AuthorizationFilterContext context)
    {
        return context.HttpContext.Items[Item.UserLogged] as User;
    }

    private static Home? GetHomeById(AuthorizationFilterContext context, Guid homeIdParsed)
    {
        var homeOwnerService = GetHomeOwnerService(context);
        try
        {
            return homeOwnerService.GetHome(homeIdParsed);
        }
        catch (ArgumentException)
        {
            return null;
        }
    }

    private static Home? GetHomeFromMember(AuthorizationFilterContext context, Guid memberIdParsed)
    {
        var homeOwnerService = GetHomeOwnerService(context);
        try
        {
            var member = homeOwnerService.GetMemberById(memberIdParsed);
            return member.Home;
        }
        catch (ArgumentException)
        {
            return null;
        }
    }

    private static bool UserHasRequiredPermission(User user, Home home, string permission)
    {
        var homePermission = new HomePermission(permission);
        return IsHomeOwner(user, home) || UserIsMemberWithPermission(user, home, homePermission);
    }

    private static bool IsHomeOwner(User user, Home home)
    {
        return home.Owner.Id == user.Id;
    }

    private static bool UserIsMemberWithPermission(User user, Home home, HomePermission homePermission)
    {
        var member = home.Members.FirstOrDefault(m => m.User.Id == user.Id);
        return member != null && member.HasPermission(homePermission);
    }

    private static IHomeOwnerService GetHomeOwnerService(AuthorizationFilterContext context)
    {
        return context.HttpContext.RequestServices.GetRequiredService<IHomeOwnerService>();
    }

    private static string? GetRouteValueAsString(AuthorizationFilterContext context, string routeKey)
    {
        return context.RouteData.Values[routeKey]?.ToString();
    }

    private static bool IsValidGuid(string guidString, out Guid parsedGuid)
    {
        return Guid.TryParse(guidString, out parsedGuid);
    }

    private static void SetUnauthorizedResult(AuthorizationFilterContext context)
    {
        SetResult(context, HttpStatusCode.Unauthorized, "Unauthorized", "You are not authenticated");
    }

    private static void SetBadRequestResult(AuthorizationFilterContext context, string message)
    {
        SetResult(context, HttpStatusCode.BadRequest, "BadRequest", message);
    }

    private static void SetNotFoundResult(AuthorizationFilterContext context, string message)
    {
        SetResult(context, HttpStatusCode.NotFound, "NotFound", message);
    }

    private static void SetForbiddenResult(AuthorizationFilterContext context, string permission)
    {
        SetResult(context, HttpStatusCode.Forbidden, "Forbidden", $"Missing home permission: {permission}");
    }

    private static void SetResult(AuthorizationFilterContext context, HttpStatusCode statusCode, string innerCode,
        string message)
    {
        context.Result = new ObjectResult(new { InnerCode = innerCode, Message = message })
        {
            StatusCode = (int)statusCode
        };
    }
}
