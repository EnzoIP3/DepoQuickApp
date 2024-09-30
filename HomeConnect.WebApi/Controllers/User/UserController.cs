using BusinessLogic;
using HomeConnect.WebApi.Filters;
using HomeConnect.WebApi.Test.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.User;

[ApiController]
[Route("users")]
[AuthorizationFilter]
public class UserController(IAdminService adminService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = adminService.GetUsers(null, null, null, null);
        var response = new
        {
            users.Data,
            Pagination = new Pagination
            {
                Page = users.Page, PageSize = users.PageSize, TotalPages = users.TotalPages
            }
        };
        return Ok(response);
    }
}
