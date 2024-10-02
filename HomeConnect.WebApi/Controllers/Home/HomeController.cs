using BusinessLogic.HomeOwners.Models;
using HomeConnect.WebApi.Filters;
using HomeConnect.WebApi.Test.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HomeConnect.WebApi.Controllers.Home;

[ApiController]
[Route("homes")]
[AuthorizationFilter]
public class HomeController(IHomeOwnerService homeOwnerService) : ControllerBase
{
    [HttpPost]
    public CreateHomeResponse CreateHome([FromBody] CreateHomeRequest request, AuthorizationFilterContext context)
    {
        var userLoggedIn = context.HttpContext.Items[Item.UserLogged];
        CreateHomeArgs createHomeArgs = HomeArgsFromRequest(request, (BusinessLogic.Users.Entities.User)userLoggedIn!);
        var homeId = homeOwnerService.Create(createHomeArgs);
        return new CreateHomeResponse { Id = homeId.ToString() };
    }

    private CreateHomeArgs HomeArgsFromRequest(CreateHomeRequest request, BusinessLogic.Users.Entities.User user)
    {
        var homeArgs = new CreateHomeArgs
        {
            HomeOwnerId = user.Id.ToString(),
            Address = request.Address,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            MaxMembers = request.MaxMembers
        };
        return homeArgs;
    }
}
