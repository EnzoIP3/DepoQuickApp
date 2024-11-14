using BusinessLogic.HomeOwners.Models;
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

    [HttpPost("name_device")]
    [AuthorizationFilter(SystemPermission.NameDevice)]
    public NameDeviceResponse NameDevice([FromBody] NameDeviceRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        var nameDeviceArgs = NameDeviceArgsFromRequest(request, userLoggedIn.Id);
        homeOwnerService.NameDevice(nameDeviceArgs);
        return new NameDeviceResponse { DeviceId = request.HardwareId };
    }

    private static NameDeviceArgs NameDeviceArgsFromRequest(NameDeviceRequest request, Guid userId)
    {
        if (string.IsNullOrEmpty(request.HardwareId))
        {
            throw new ArgumentException("DeviceId cannot be null or empty");
        }

        if (string.IsNullOrEmpty(request.NewName))
        {
            throw new ArgumentException("NewName cannot be null or empty");
        }

        return new NameDeviceArgs()
        {
            HardwareId = Guid.Parse(request.HardwareId), NewName = request.NewName, OwnerId = userId
        };
    }
}
