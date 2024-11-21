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
public class BusinessOwnerController
    : ControllerBase
{
    private readonly IUserService _userService;

    public BusinessOwnerController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [AuthorizationFilter(SystemPermission.CreateBusinessOwner)]
    public CreateBusinessOwnerResponse CreateBusinessOwner(CreateBusinessOwnerRequest request)
    {
        User businessOwner = _userService.CreateUser(request.ToCreateUserArgs());
        return new CreateBusinessOwnerResponse { Id = businessOwner.Id.ToString() };
    }
}
