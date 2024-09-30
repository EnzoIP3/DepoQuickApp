using BusinessLogic;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.BusinessOwner;

[ApiController]
[Route("business_owners")]
[AuthorizationFilter]
public class BusinessOwnerController(IAdminService adminService) : ControllerBase
{
    public CreateBusinessOwnerResponse CreateBusinessOwner(CreateBusinessOwnerRequest request, string s)
    {
        var userModel = new UserModel
        {
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            Password = request.Password
        };
        var businessOwnerId = adminService.CreateBusinessOwner(userModel);
        return new CreateBusinessOwnerResponse() { Id = businessOwnerId.ToString() };
    }
}
