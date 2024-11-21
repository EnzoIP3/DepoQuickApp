using BusinessLogic;
using BusinessLogic.Admins.Services;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Models;
using BusinessLogic.Users.Services;
using HomeConnect.WebApi.Controllers.Businesses.Models;
using HomeConnect.WebApi.Controllers.Users.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Users;

[ApiController]
[Route("users")]
[AuthenticationFilter]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAdminService _adminService;
    private readonly IBusinessOwnerService _businessOwnerService;

    public UserController(IUserService userService, IAdminService adminService,
        IBusinessOwnerService businessOwnerService)
    {
        _userService = userService;
        _adminService = adminService;
        _businessOwnerService = businessOwnerService;
    }

    [HttpGet]
    [AuthorizationFilter(SystemPermission.GetAllUsers)]
    public GetUsersResponse GetUsers([FromQuery] GetUsersRequest request)
    {
        PagedData<User> users = _adminService.GetUsers(request.ToArgs());
        return GetUsersResponse.FromUsers(users);
    }

    [HttpPatch("home_owner_role")]
    public AddHomeOwnerRoleResponse AddHomeOwnerRole()
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        var args = new AddRoleToUserArgs { UserId = userLoggedIn!.Id.ToString(), Role = "HomeOwner" };
        _userService.AddRoleToUser(args);
        var user = _userService.AddRoleToUser(args);
        return new AddHomeOwnerRoleResponse
        {
            Id = args.UserId,
            Roles = user.GetRolesAndPermissions()
                .ToDictionary(x => x.Key.Name, x => x.Value.Select(y => y.Value).ToList())
        };
    }

    [HttpGet("{userId}/businesses")]
    [AuthorizationFilter(SystemPermission.GetBusinesses)]
    public GetBusinessesResponse GetBusinesses(string userId, [FromQuery] GetUserBusinessesRequest request)
    {
        PagedData<Business> businesses =
            _businessOwnerService.GetBusinesses(userId, request.CurrentPage, request.PageSize);
        return GetBusinessesResponse.FromBusinesses(businesses);
    }
}
