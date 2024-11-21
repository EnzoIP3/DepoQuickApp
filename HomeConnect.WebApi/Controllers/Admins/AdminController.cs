using BusinessLogic.Admins.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Services;
using HomeConnect.WebApi.Controllers.Admins.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Admins;

[ApiController]
[Route("admins")]
[AuthenticationFilter]
public sealed class AdminController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAdminService _adminService;

    public AdminController(IUserService userService, IAdminService adminService)
    {
        _userService = userService;
        _adminService = adminService;
    }

    [HttpPost]
    [AuthorizationFilter(SystemPermission.CreateAdministrator)]
    public CreateAdminResponse CreateAdmin([FromBody] CreateAdminRequest request)
    {
        var admin = _userService.CreateUser(request.ToCreateUserArgs());
        return new CreateAdminResponse { Id = admin.Id.ToString() };
    }

    [HttpDelete("{adminId}")]
    [AuthorizationFilter(SystemPermission.DeleteAdministrator)]
    public DeleteAdminResponse DeleteAdmin([FromRoute] string adminId)
    {
        _adminService.DeleteAdmin(adminId);
        return new DeleteAdminResponse { Id = adminId };
    }
}
