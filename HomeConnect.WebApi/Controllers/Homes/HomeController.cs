using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Models;
using BusinessLogic.HomeOwners.Services;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using HomeConnect.WebApi.Controllers.HomeOwners.Models;
using HomeConnect.WebApi.Controllers.Homes.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Homes;

[ApiController]
[Route("homes")]
[AuthenticationFilter]
public sealed class HomeController : ControllerBase
{
    private readonly IHomeOwnerService _homeOwnerService;

    public HomeController(IHomeOwnerService homeOwnerService)
    {
        _homeOwnerService = homeOwnerService;
    }

    [HttpGet]
    [AuthorizationFilter(SystemPermission.GetHomes)]
    public GetHomesResponse GetHomes()
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        List<Home> homes = _homeOwnerService.GetHomesByOwnerId(userLoggedIn!.Id);
        return GetHomesResponse.FromHomes(homes);
    }

    [HttpGet("{homesId}/permissions")]
    [HomeAuthorizationFilter]
    [AuthorizationFilter(SystemPermission.GetHomes)]
    public GetHomePermissionsResponse GetHomePermissions([FromRoute] string homesId)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        var permissions = _homeOwnerService.GetHomePermissions(Guid.Parse(homesId), userLoggedIn!.Id);
        return GetHomePermissionsResponse.FromHomePermissions(homesId, permissions);
    }

    [HttpPatch("{homesId}/name")]
    [HomeAuthorizationFilter(HomePermission.NameHome)]
    [AuthorizationFilter(SystemPermission.NameHome)]
    public NameHomeResponse NameHome([FromRoute] string homesId, [FromBody] NameHomeRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        _homeOwnerService.NameHome(request.ToArgs(homesId, userLoggedIn!));
        return new NameHomeResponse { HomeId = homesId };
    }

    [HttpGet("{homesId}")]
    [HomeAuthorizationFilter(HomePermission.GetHome)]
    [AuthorizationFilter(SystemPermission.GetHomes)]
    public GetHomeResponse GetHome([FromRoute] string homesId)
    {
        var home = _homeOwnerService.GetHome(Guid.Parse(homesId));
        return GetHomeResponse.FromHome(home);
    }

    [HttpPost]
    [AuthorizationFilter(SystemPermission.CreateHome)]
    public CreateHomeResponse CreateHome([FromBody] CreateHomeRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        Guid homeId = _homeOwnerService.CreateHome(request.ToArgs(userLoggedIn!));
        return new CreateHomeResponse { Id = homeId.ToString() };
    }

    [HttpPost("{homesId}/members")]
    [AuthorizationFilter(SystemPermission.AddMember)]
    [HomeAuthorizationFilter(HomePermission.AddMember)]
    public AddMemberResponse AddMember([FromRoute] string homesId, [FromBody] AddMemberRequest request)
    {
        Guid addedMemberId = _homeOwnerService.AddMemberToHome(request.ToArgs(homesId));
        return new AddMemberResponse { HomeId = homesId, MemberId = addedMemberId.ToString() };
    }

    [HttpGet("{homesId}/members")]
    [AuthorizationFilter(SystemPermission.GetMembers)]
    [HomeAuthorizationFilter(HomePermission.GetMembers)]
    public GetMembersResponse GetMembers([FromRoute] string homesId)
    {
        List<Member> members = _homeOwnerService.GetHomeMembers(homesId);
        return GetMembersResponse.FromMembers(members);
    }

    [HttpGet("{homesId}/devices")]
    [AuthorizationFilter(SystemPermission.GetDevices)]
    [HomeAuthorizationFilter(HomePermission.GetDevices)]
    public GetHomeDevicesResponse GetDevices([FromRoute] string homesId, [FromQuery] string? roomId = null)
    {
        IEnumerable<OwnedDevice> devices = _homeOwnerService.GetHomeDevices(homesId, roomId);
        return GetHomeDevicesResponse.FromDevices(devices);
    }

    [HttpPost("{homesId}/devices")]
    [AuthorizationFilter(SystemPermission.AddDevice)]
    [HomeAuthorizationFilter(HomePermission.AddDevice)]
    public AddDevicesResponse AddDevices([FromRoute] string homesId, [FromBody] AddDevicesRequest request)
    {
        _homeOwnerService.AddDeviceToHome(request.ToArgs(homesId));
        return new AddDevicesResponse { HomeId = homesId, DeviceIds = request.DeviceIds! };
    }

    [HttpPost("{homesId}/rooms")]
    [AuthorizationFilter(SystemPermission.CreateRoom)]
    [HomeAuthorizationFilter(HomePermission.CreateRoom)]
    public CreateRoomResponse CreateRoom([FromRoute] string homesId, [FromBody] CreateRoomRequest request)
    {
        var room = _homeOwnerService.CreateRoom(homesId, request.Name ?? string.Empty);
        return new CreateRoomResponse { RoomId = room.Id.ToString() };
    }

    [HttpGet("{homesId}/rooms")]
    public GetRoomsResponse GetRooms([FromRoute] string homesId)
    {
        IEnumerable<Room> rooms = _homeOwnerService.GetRoomsByHomeId(homesId);
        return GetRoomsResponse.FromRooms(rooms);
    }
}
