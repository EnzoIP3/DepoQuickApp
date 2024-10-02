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
        var homeId = homeOwnerService.CreateHome(createHomeArgs);
        return new CreateHomeResponse { Id = homeId.ToString() };
    }

    [HttpPost("{homesId}/members")]
    public AddMemberResponse AddMember(string homesId, [FromBody] AddMemberRequest request, AuthorizationFilterContext context)
    {
        var userLoggedIn = context.HttpContext.Items[Item.UserLogged];
        var addMemberArgs = ArgsFromRequest(request, homesId, (BusinessLogic.Users.Entities.User)userLoggedIn!);
        var addedMemberId = homeOwnerService.AddMemberToHome(addMemberArgs);
        return new AddMemberResponse { HomeId = homesId, MemberId = addedMemberId.ToString() };
    }

    private AddMemberArgs ArgsFromRequest(AddMemberRequest request, string homesId, BusinessLogic.Users.Entities.User userLoggedIn)
    {
        var addMemberArgs = new AddMemberArgs
        {
            HomeId = homesId,
            HomeOwnerId = userLoggedIn.Id.ToString(),
            CanAddDevices = request.CanAddDevices,
            CanListDevices = request.CanListDevices
        };
        return addMemberArgs;
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
