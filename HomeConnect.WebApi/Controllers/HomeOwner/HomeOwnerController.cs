using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Models;
using BusinessLogic.Users.Services;
using HomeConnect.WebApi.Controllers.HomeOwner.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.HomeOwner;

[ApiController]
[Route("home_owners")]
[AuthenticationFilter]
public class HomeOwnerController(IUserService userService) : ControllerBase
{
    [HttpPost]
    public CreateHomeOwnerResponse CreateHomeOwner([FromBody] CreateHomeOwnerRequest args)
    {
        var user = userService.CreateUser(new CreateUserArgs
        {
            Name = args.Name,
            Surname = args.Surname,
            Email = args.Email,
            Password = args.Password,
            Role = Role.HomeOwner,
            ProfilePicture = args.ProfilePicture
        });
        return new CreateHomeOwnerResponse { Id = user.Id.ToString() };
    }
}
