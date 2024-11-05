using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using BusinessLogic.Users.Models;
using BusinessLogic.Users.Services;
using HomeConnect.WebApi.Controllers.HomeOwners.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.HomeOwners;

[ApiController]
[Route("home_owners")]
public class HomeOwnerController(IUserService userService, IHomeOwnerService homeOwnerService) : ControllerBase
{
    [HttpPost]
    public CreateHomeOwnerResponse CreateHomeOwner([FromBody] CreateHomeOwnerRequest args)
    {
        User user = userService.CreateUser(new CreateUserArgs
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

    [HttpGet("homes")]
    [AuthorizationFilter(SystemPermission.GetHomes)]
    public GetHomesResponse GetHomes()
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        List<Home> homes = homeOwnerService.GetHomesByOwnerId(userLoggedIn.Id);
        var homeInfos = homes.Select(h => new ListHomeInfo
        {
            Id = h.Id.ToString(),
            Address = h.Address,
            Latitude = h.Latitude,
            Longitude = h.Longitude,
            MaxMembers = h.MaxMembers
        }).ToList();
        return new GetHomesResponse { Homes = homeInfos };
    }

    [HttpPost("name_home")]
    [AuthorizationFilter(SystemPermission.NameHome)]
    public NameHomeResponse NameHome([FromBody] NameHomeRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        var nameHomeArgs = NameHomeArgsFromRequest(request);
        homeOwnerService.NameHome(userLoggedIn.Id, nameHomeArgs.HomeId, nameHomeArgs.NewName);
        return new NameHomeResponse { HomeId = request.HomeId };
    }

    [HttpPost("name_device")]
    [AuthorizationFilter(SystemPermission.NameDevice)]
    public NameDeviceResponse NameDevice([FromBody] NameDeviceRequest request)
    {
        throw new NotImplementedException();
    }

    private static (Guid HomeId, string NewName) NameHomeArgsFromRequest(NameHomeRequest request)
    {
        if (string.IsNullOrEmpty(request.HomeId))
        {
            throw new ArgumentException("HomeId cannot be null or empty");
        }

        if (string.IsNullOrEmpty(request.NewName))
        {
            throw new ArgumentException("NewName cannot be null or empty");
        }

        return (Guid.Parse(request.HomeId), request.NewName);
    }
}
