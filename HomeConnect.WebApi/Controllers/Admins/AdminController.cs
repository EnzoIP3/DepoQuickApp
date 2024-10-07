using BusinessLogic.Admins.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Models;
using HomeConnect.WebApi.Controllers.Admin.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Admin;

[ApiController]
[Route("admins")]
[AuthenticationFilter]
public class AdminController(IAdminService adminService) : ControllerBase
{
    [HttpPost]
    [AuthorizationFilter(SystemPermission.CreateAdministrator)]
    public CreateAdminResponse CreateAdmin([FromBody] CreateAdminRequest request)
    {
        CreateUserArgs createUserArgs = UserModelFromRequest(request);
        Guid adminId = adminService.Create(createUserArgs);
        return new CreateAdminResponse { Id = adminId.ToString() };
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
        adminService.Delete(adminId);
        return new DeleteAdminResponse { Id = adminId };
    }
}
