using BusinessLogic.Users.Models;
using BusinessLogic.Users.Services;
using HomeConnect.WebApi.Controllers.HomeOwner.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.HomeOwner;

[ApiController]
[Route("home_owners")]
[AuthorizationFilter]
public class HomeOwnerController(IUserService userService) : ControllerBase
{
    public IUserService UserService { get; } = userService;

    [HttpPost]
    public CreateHomeOwnerResponse CreateHomeOwner([FromBody] CreateHomeOwnerRequest args)
    {
        var user = UserService.CreateUser(new CreateUserArgs { Email = args.Email, Password = args.Password });
        return new CreateHomeOwnerResponse { Id = user.Id.ToString() };
    }
}
