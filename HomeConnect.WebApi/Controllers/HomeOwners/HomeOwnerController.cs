using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Services;
using HomeConnect.WebApi.Controllers.HomeOwners.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.HomeOwners;

[ApiController]
[Route("home_owners")]
public sealed class HomeOwnerController : ControllerBase
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
