using BusinessLogic;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Admin;

[ApiController]
[Route("admins")]
[AuthorizationFilter]
public class AdminController(IAdminService adminService) : ControllerBase
{
    [HttpPost]
    public CreateAdminResponse CreateAdmin([FromBody] CreateAdminRequest request, [FromHeader] string authorization)
    {
        UserModel userModel = UserModelFromRequest(request);
        var adminId = adminService.Create(userModel);
        return new CreateAdminResponse { Id = adminId.ToString() };
    }

    private static UserModel UserModelFromRequest(CreateAdminRequest request)
    {
        var userModel = new UserModel
        {
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            Password = request.Password
        };
        return userModel;
    }

    [HttpDelete("{adminId}")]
    public IActionResult DeleteAdmin([FromRoute] Guid adminId, [FromHeader] string authorization)
    {
        adminService.Delete(adminId);
        return NoContent();
    }
}
