using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Models;
using BusinessLogic.Users.Services;
using HomeConnect.WebApi.Controllers.BusinessOwners.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.BusinessOwners;

[ApiController]
[Route("business_owners")]
[AuthenticationFilter]
public class BusinessOwnerController(IUserService userService)
    : ControllerBase
{
    [HttpPost]
    [AuthorizationFilter(SystemPermission.CreateBusinessOwner)]
    public CreateBusinessOwnerResponse CreateBusinessOwner(CreateBusinessOwnerRequest request)
    {
        CreateUserArgs createUserArgs = UserModelFromRequest(request);
        User businessOwner = userService.CreateUser(createUserArgs);
        return new CreateBusinessOwnerResponse { Id = businessOwner.Id.ToString() };
    }

    private static CreateUserArgs UserModelFromRequest(CreateBusinessOwnerRequest request)
    {
        var userModel = new CreateUserArgs
        {
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            Password = request.Password,
            Role = Role.BusinessOwner
        };
        return userModel;
    }
}
