using BusinessLogic;
using BusinessLogic.Admins.Services;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Users.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.BusinessOwner;

[ApiController]
[Route("business_owners")]
[AuthorizationFilter]
public class BusinessOwnerController(IAdminService adminService, IBusinessOwnerService businessOwnerService) : ControllerBase
{
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

    [HttpPost]
    [Route("businesses")]
    public CreateBusinessResponse CreateBusiness([FromBody] CreateBusinessRequest request, [FromHeader] string authorization)
    {
        var business = new BusinessLogic.BusinessOwners.Entities.Business
        {
            Name = request.Name,
            Rut = request.Rut,
            Owner = request.Owner,
        };

        var createdBusiness = businessOwnerService.CreateBusiness(business.Owner.Email, business.Rut, business.Name);

        return new CreateBusinessResponse { Id = createdBusiness };
    }
}
