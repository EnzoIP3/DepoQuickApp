using BusinessLogic.Admins.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Models;
using BusinessLogic.Users.Services;
using HomeConnect.WebApi.Controllers.Admins.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Admins;

[ApiController]
[Route("admins")]
[AuthenticationFilter]
public class AdminController(IUserService userService, IAdminService adminService) : ControllerBase
{
    [HttpPost]
    [AuthorizationFilter(SystemPermission.CreateAdministrator)]
    public CreateAdminResponse CreateAdmin([FromBody] CreateAdminRequest request)
    {
        CreateUserArgs createUserArgs = UserModelFromRequest(request);
        var admin = userService.CreateUser(createUserArgs);
        return new CreateAdminResponse { Id = admin.Id.ToString() };
    }

    private static CreateUserArgs UserModelFromRequest(CreateAdminRequest request)
    {
        var userModel = new CreateUserArgs
        {
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            Password = request.Password,
            Role = Role.Admin
        };
        return userModel;
    }

    [HttpDelete("{adminId}")]
    [AuthorizationFilter(SystemPermission.DeleteAdministrator)]
    public DeleteAdminResponse DeleteAdmin([FromRoute] string adminId)
    {
        adminService.DeleteAdmin(adminId);
        return new DeleteAdminResponse { Id = adminId };
    }
}
