using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Models;
using BusinessLogic.Users.Services;
using HomeConnect.WebApi.Controllers.HomeOwners.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace HomeConnect.WebApi.Controllers.HomeOwners;

[ApiController]
[Route("home_owners")]
public class HomeOwnerController : ControllerBase
{
    private readonly IUserService _userService;

    public HomeOwnerController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public CreateHomeOwnerResponse CreateHomeOwner([FromBody] CreateHomeOwnerRequest request)
    {
        User user = _userService.CreateUser(request.ToArgs());
        return new CreateHomeOwnerResponse { Id = user.Id.ToString() };
    }
}
