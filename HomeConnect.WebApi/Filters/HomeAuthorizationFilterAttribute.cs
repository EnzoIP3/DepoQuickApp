using System.Net;
using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Services;
using BusinessLogic.Users.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HomeConnect.WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class HomeAuthorizationFilterAttribute(string? permission = null) : Attribute, IAuthorizationFilter
{
    private const string HomeIdRoute = "homesId";
    private const string MemberIdRoute = "membersId";
    private const string HardwareIdRoute = "hardwareId";
    private const string RoomIdRoute = "roomId";

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        User? user = GetAuthenticatedUser(context);
        if (user == null)
        {
            SetUnauthorizedResult(context);
            return;
        }

        var homeId = GetRouteValueAsString(context, HomeIdRoute);
        var memberId = GetRouteValueAsString(context, MemberIdRoute);
        var hardwareId = GetRouteValueAsString(context, HardwareIdRoute);
        var roomId = GetRouteValueAsString(context, RoomIdRoute);

        if (homeId != null)
        {
            HandleHomeId(context, homeId, user);
            return;
        }

        if (memberId != null)
        {
            HandleMemberId(context, memberId, user);
            return;
        }

        if (hardwareId != null)
        {
            HandleHardwareId(context, hardwareId, user);
            return;
        }

        if (roomId != null)
        {
            HandleRoomId(context, roomId, user);
        }
    }

    private void HandleRoomId(AuthorizationFilterContext context, string roomId, User user)
    {
        if (!IsValidGuid(roomId, out Guid _))
        {
            SetBadRequestResult(context, "The room ID is invalid");
            return;
        }

        IHomeOwnerService homeOwnerService = GetHomeOwnerService(context);
        try
        {
            Room room = homeOwnerService.GetRoom(roomId);
            Home home = room.Home;
            EnsureUserHasRequiredPermission(context, user, home);
        }
        catch (KeyNotFoundException)
        {
            SetNotFoundResult(context, "The room does not exist");
        }
    }

    private void EnsureUserHasRequiredPermission(AuthorizationFilterContext context, User user, Home home)
    {
        if (!UserHasRequiredPermission(user, home, permission))
        {
            SetForbiddenResult(context, permission);
        }
    }

    private void HandleHardwareId(AuthorizationFilterContext context, string hardwareId, User user)
    {
        if (!IsValidGuid(hardwareId, out Guid _))
        {
            SetBadRequestResult(context, "The hardware ID is invalid");
            return;
        }

        IHomeOwnerService homeOwnerService = GetHomeOwnerService(context);
        try
        {
            OwnedDevice device = homeOwnerService.GetOwnedDeviceByHardwareId(hardwareId);
            Home home = device.Home;
            EnsureUserHasRequiredPermission(context, user, home);
        }
        catch (KeyNotFoundException)
        {
            SetNotFoundResult(context, "The device does not exist");
        }
    }

    private void HandleMemberId(AuthorizationFilterContext context, string memberId, User user)
    {
        if (!IsValidGuid(memberId, out Guid memberIdParsed))
        {
            SetBadRequestResult(context, "The member ID is invalid");
            return;
        }

        Home? home = GetHomeFromMember(context, memberIdParsed);
        if (!ValidateHomeExistence(context, home, "The member does not exist"))
        {
            return;
        }

        EnsureUserHasRequiredPermission(context, user, home!);
    }

    private void HandleHomeId(AuthorizationFilterContext context, string homeId, User user)
    {
        if (!IsValidGuid(homeId, out Guid homeIdParsed))
        {
            SetBadRequestResult(context, "The home ID is invalid");
            return;
        }

        Home? home = GetHomeById(context, homeIdParsed);
        if (!ValidateHomeExistence(context, home, "The home does not exist"))
        {
            return;
        }

        EnsureUserHasRequiredPermission(context, user, home!);
    }

    private static bool ValidateHomeExistence(AuthorizationFilterContext context, Home? home, string notFoundMessage)
    {
        if (home != null)
        {
            return true;
        }

        SetNotFoundResult(context, notFoundMessage);
        return false;
    }

    private static User? GetAuthenticatedUser(AuthorizationFilterContext context)
    {
        return context.HttpContext.Items[Item.UserLogged] as User;
    }

    private static Home? GetHomeById(AuthorizationFilterContext context, Guid homeIdParsed)
    {
        IHomeOwnerService homeOwnerService = GetHomeOwnerService(context);
        try
        {
            return homeOwnerService.GetHome(homeIdParsed);
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
    }

    private static Home? GetHomeFromMember(AuthorizationFilterContext context, Guid memberIdParsed)
    {
        IHomeOwnerService homeOwnerService = GetHomeOwnerService(context);
        try
        {
            Member member = homeOwnerService.GetMemberById(memberIdParsed);
            return member.Home;
        }
        catch (ArgumentException)
        {
            return null;
        }
    }

    private static bool UserHasRequiredPermission(User user, Home home, string? permission)
    {
        var homePermission = new HomePermission(permission ?? string.Empty);
        return IsHomeOwner(user, home) || UserIsMemberWithPermission(user, home, homePermission);
    }

    private static bool IsHomeOwner(User user, Home home)
    {
        return home.Owner.Id == user.Id;
    }

    private static bool UserIsMemberWithPermission(User user, Home home, HomePermission homePermission)
    {
        Member? member = home.Members.FirstOrDefault(m => m.User.Id == user.Id);
        return member != null && (member.HasPermission(homePermission) || homePermission.Value == string.Empty);
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

    private static void SetForbiddenResult(AuthorizationFilterContext context, string? permission)
    {
        if (permission == null)
        {
            SetResult(context, HttpStatusCode.Forbidden, "Forbidden",
                "You do not belong to this home");
            return;
        }

        SetResult(context, HttpStatusCode.Forbidden, "Forbidden", "You are not allowed to do that in this home");
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
