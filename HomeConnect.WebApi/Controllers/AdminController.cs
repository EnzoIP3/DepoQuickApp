using BusinessLogic;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers;

[ApiController]
[Route("admins")]
[AuthorizationFilter]
public class AdminController(IAdminService adminService) : ControllerBase
{
    [HttpPost]
    public CreateAdminResponse CreateAdmin([FromBody] CreateAdminRequest request, [FromHeader] string authorization)
    {
        var userModel = new UserModel
        {
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            Password = request.Password
        };
        var adminId = adminService.Create(userModel);
        return new CreateAdminResponse { Id = adminId.ToString() };
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteAdmin([FromRoute] Guid id, [FromHeader] string authorization)
    {
        adminService.Delete(id);
        return NoContent();
    }
}
