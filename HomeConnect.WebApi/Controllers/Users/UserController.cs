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
public class UserController(IAdminService adminService, IUserService userService, IBusinessOwnerService businessOwnerService) : ControllerBase
{
    [HttpGet]
    [AuthorizationFilter(SystemPermission.GetAllUsers)]
    public GetUsersResponse GetUsers([FromQuery] GetUsersRequest request)
    {
        PagedData<User> users = adminService.GetUsers(request.CurrentPage,
            request.PageSize, request.FullName,
            request.Roles);
        GetUsersResponse response = ResponseFromUsers(users);
        return response;
    }

    private static GetUsersResponse ResponseFromUsers(PagedData<User> users)
    {
        var response = new GetUsersResponse
        {
            Users = users.Data.Select(user => new ListUserInfo
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Surname = user.Surname,
                Roles = user.Roles.Select(r => r.Name).ToList(),
                CreatedAt = user.CreatedAt.ToString()
            }).ToList(),
            Pagination = new Pagination
            {
                Page = users.Page,
                PageSize = users.PageSize,
                TotalPages = users.TotalPages
            }
        };
        return response;
    }

    [HttpPatch("home_owner_role")]
    public AddHomeOwnerRoleResponse AddHomeOwnerRole()
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        var args = new AddRoleToUserArgs { UserId = userLoggedIn!.Id.ToString(), Role = "HomeOwner" };
        userService.AddRoleToUser(args);
        return new AddHomeOwnerRoleResponse { Id = args.UserId };
    }

    [HttpGet("{userId}/businesses")]
    [AuthorizationFilter(SystemPermission.GetBusinesses)]
    public GetBusinessesResponse GetBusinesses(string userId)
    {
        PagedData<Business> businesses = businessOwnerService.GetBusinesses(userId);
        GetBusinessesResponse response = ResponseFromBusinesses(businesses);
        return response;
    }

    private GetBusinessesResponse ResponseFromBusinesses(PagedData<Business> businesses)
    {
        return new GetBusinessesResponse
        {
            Businesses = businesses.Data.Select(b => new ListBusinessInfo
            {
                Name = b.Name,
                OwnerEmail = b.Owner.Email,
                OwnerName = b.Owner.Name,
                OwnerSurname = b.Owner.Surname,
                Rut = b.Rut
            }).ToList(),
            Pagination = new Pagination
            {
                Page = businesses.Page,
                PageSize = businesses.PageSize,
                TotalPages = businesses.TotalPages
            }
        };
    }
}
