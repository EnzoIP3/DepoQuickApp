using BusinessLogic;
using BusinessLogic.Admins.Services;
using BusinessLogic.Roles.Entities;
using HomeConnect.WebApi.Controllers.User.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.User;

[ApiController]
[Route("users")]
[AuthenticationFilter]
public class UserController(IAdminService adminService) : ControllerBase
{
    [HttpGet]
    [AuthorizationFilter(SystemPermission.GetAllUsers)]
    public GetUsersResponse GetUsers([FromQuery] GetUsersRequest request)
    {
        PagedData<BusinessLogic.Users.Entities.User> users = adminService.GetUsers(request.CurrentPage,
            request.PageSize, request.FullName,
            request.Role);
        GetUsersResponse response = ResponseFromUsers(users);
        return response;
    }

    private static GetUsersResponse ResponseFromUsers(PagedData<BusinessLogic.Users.Entities.User> users)
    {
        var response = new GetUsersResponse
        {
            Users = users.Data.Select(user => new ListUserInfo
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Surname = user.Surname,
                Role = user.RoleName,
                CreatedAt = user.CreatedAt.ToString()
            }).ToList(),
            Pagination = new Pagination
            {
                Page = users.Page, PageSize = users.PageSize, TotalPages = users.TotalPages
            }
        };
        return response;
    }
}
