using BusinessLogic;
using BusinessLogic.Admins.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Models;
using BusinessLogic.Users.Services;
using HomeConnect.WebApi.Controllers.Users.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Users;

[ApiController]
[Route("users")]
[AuthenticationFilter]
public class UserController(IAdminService adminService, IUserService userService) : ControllerBase
{
    public IUserService UserService { get; } = userService;

    [HttpGet]
    [AuthorizationFilter(SystemPermission.GetAllUsers)]
    public GetUsersResponse GetUsers([FromQuery] GetUsersRequest request)
    {
        PagedData<User> users = adminService.GetUsers(request.CurrentPage,
            request.PageSize, request.FullName,
            request.Role);
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
                Page = users.Page, PageSize = users.PageSize, TotalPages = users.TotalPages
            }
        };
        return response;
    }

    [HttpPatch("/home_owner_role")]
    public AddHomeOwnerRoleResponse AddHomeOwnerRole()
    {
        throw new NotImplementedException();
    }
}
