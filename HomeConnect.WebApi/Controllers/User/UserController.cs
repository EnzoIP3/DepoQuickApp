using BusinessLogic;
using BusinessLogic.Admins.Services;
using HomeConnect.WebApi.Controllers.User.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.User;

[ApiController]
[Route("users")]
[AuthorizationFilter]
public class UserController(IAdminService adminService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetUsers([FromQuery] UserQueryParameters parameters)
    {
        var users = adminService.GetUsers(parameters.CurrentPage, parameters.PageSize, parameters.FullNameFilter, parameters.RoleFilter);
        var response = ResponseFromUsers(users);
        return Ok(response);
    }

    private static object ResponseFromUsers(PagedData<BusinessLogic.Users.Entities.User> users)
    {
        var response = new
        {
            users.Data,
            Pagination = new Pagination
            {
                Page = users.Page,
                PageSize = users.PageSize,
                TotalPages = users.TotalPages
            }
        };
        return response;
    }
}
