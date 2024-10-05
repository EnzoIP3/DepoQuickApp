using BusinessLogic.Admins.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Models;
using HomeConnect.WebApi.Controllers.BusinessOwner.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.BusinessOwner;

[ApiController]
[Route("business_owners")]
[AuthenticationFilter]
public class BusinessOwnerController(IAdminService adminService)
    : ControllerBase
{
    [HttpPost]
    [AuthorizationFilter(SystemPermission.CreateAdministrator)]
    public CreateBusinessOwnerResponse CreateBusinessOwner(CreateBusinessOwnerRequest request)
    {
        CreateUserArgs createUserArgs = UserModelFromRequest(request);
        var businessOwnerId = adminService.CreateBusinessOwner(createUserArgs);
        return new CreateBusinessOwnerResponse() { Id = businessOwnerId.ToString() };
    }

    private static CreateUserArgs UserModelFromRequest(CreateBusinessOwnerRequest request)
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
}
