using BusinessLogic;
using BusinessLogic.Admins.Services;
using BusinessLogic.Users.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Admin;

[ApiController]
[Route("admins")]
[AuthorizationFilter]
public class AdminController(IAdminService adminService) : ControllerBase
{
    [HttpPost]
    public CreateAdminResponse CreateAdmin([FromBody] CreateAdminRequest request)
    {
        CreateUserArgs createUserArgs = UserModelFromRequest(request);
        var adminId = adminService.Create(createUserArgs);
        return new CreateAdminResponse { Id = adminId.ToString() };
    }

    private static CreateUserArgs UserModelFromRequest(CreateAdminRequest request)
    {
        var userModel = new CreateUserArgs
        {
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            Password = request.Password
        };
        return userModel;
    }

    [HttpDelete("{adminId}")]
    public IActionResult DeleteAdmin([FromRoute] Guid adminId)
    {
        adminService.Delete(adminId);
        return NoContent();
    }
}
