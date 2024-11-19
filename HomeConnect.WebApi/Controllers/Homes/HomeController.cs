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
public class HomeController(IHomeOwnerService homeOwnerService) : ControllerBase
{
    [HttpGet]
    [AuthorizationFilter(SystemPermission.GetHomes)]
    public GetHomesResponse GetHomes()
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        List<Home> homes = homeOwnerService.GetHomesByOwnerId(userLoggedIn!.Id);
        var homeInfos = homes.Select(h => new ListHomeInfo
        {
            Id = h.Id.ToString(),
            Name = h.NickName,
            Address = h.Address,
            Latitude = h.Latitude,
            Longitude = h.Longitude,
            MaxMembers = h.MaxMembers
        }).ToList();
        return new GetHomesResponse { Homes = homeInfos };
    }

    [HttpGet("{homesId}/permissions")]
    [HomeAuthorizationFilter]
    [AuthorizationFilter(SystemPermission.GetHomes)]
    public GetHomePermissionsResponse GetHomePermissions([FromRoute] string homesId)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        var permissions = homeOwnerService.GetHomePermissions(Guid.Parse(homesId), userLoggedIn!.Id);
        return new GetHomePermissionsResponse
        {
            HomeId = homesId, HomePermissions = permissions.Select(p => p.Value).ToList()
        };
    }

    [HttpPatch("{homesId}/name")]
    [HomeAuthorizationFilter(HomePermission.NameHome)]
    [AuthorizationFilter(SystemPermission.NameHome)]
    public NameHomeResponse NameHome([FromRoute] string homesId, [FromBody] NameHomeRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged] as User;
        var nameHomeArgs = NameHomeArgsFromRequest(request, homesId);
        nameHomeArgs.OwnerId = userLoggedIn!.Id;
        homeOwnerService.NameHome(nameHomeArgs);
        return new NameHomeResponse { HomeId = homesId };
    }

    [HttpGet("{homesId}")]
    [HomeAuthorizationFilter(HomePermission.GetHome)]
    [AuthorizationFilter(SystemPermission.GetHomes)]
    public GetHomeResponse GetHome([FromRoute] string homesId)
    {
        var home = homeOwnerService.GetHome(Guid.Parse(homesId));
        return new GetHomeResponse
        {
            Id = home.Id.ToString(),
            Name = home.NickName,
            Address = home.Address,
            Latitude = home.Latitude,
            Longitude = home.Longitude,
            MaxMembers = home.MaxMembers,
            Owner = new OwnerInfo
            {
                Id = home.Owner.Id.ToString(),
                Name = home.Owner.Name,
                Surname = home.Owner.Surname,
                ProfilePicture = home.Owner.ProfilePicture
            }
        };
    }

    private static NameHomeArgs NameHomeArgsFromRequest(NameHomeRequest request, string homesId)
    {
        if (string.IsNullOrEmpty(homesId))
        {
            throw new ArgumentException("HomeId cannot be null or empty");
        }

        if (string.IsNullOrEmpty(request.NewName))
        {
            throw new ArgumentException("NewName cannot be null or empty");
        }

        return new NameHomeArgs { HomeId = Guid.Parse(homesId), NewName = request.NewName };
    }

    [HttpPost]
    [AuthorizationFilter(SystemPermission.CreateHome)]
    public CreateHomeResponse CreateHome([FromBody] CreateHomeRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged];
        CreateHomeArgs createHomeArgs = HomeArgsFromRequest(request, (User)userLoggedIn!);
        Guid homeId = homeOwnerService.CreateHome(createHomeArgs);
        return new CreateHomeResponse { Id = homeId.ToString() };
    }

    [HttpPost("{homesId}/members")]
    [AuthorizationFilter(SystemPermission.AddMember)]
    [HomeAuthorizationFilter(HomePermission.AddMember)]
    public AddMemberResponse AddMember([FromRoute] string homesId, [FromBody] AddMemberRequest request)
    {
        var addMemberArgs = new AddMemberArgs
        {
            HomeId = homesId,
            UserEmail = request.Email ?? string.Empty,
            Permissions = request.Permissions
        };
        Guid addedMemberId = homeOwnerService.AddMemberToHome(addMemberArgs);
        return new AddMemberResponse { HomeId = homesId, MemberId = addedMemberId.ToString() };
    }

    private CreateHomeArgs HomeArgsFromRequest(CreateHomeRequest request, User user)
    {
        var homeArgs = new CreateHomeArgs
        {
            HomeOwnerId = user.Id.ToString(),
            Address = request.Address ?? string.Empty,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            MaxMembers = request.MaxMembers,
            Name = request.Name
        };
        return homeArgs;
    }

    [HttpGet("{homesId}/members")]
    [AuthorizationFilter(SystemPermission.GetMembers)]
    [HomeAuthorizationFilter(HomePermission.GetMembers)]
    public GetMembersResponse GetMembers([FromRoute] string homesId)
    {
        List<Member> members = homeOwnerService.GetHomeMembers(homesId);
        var memberInfos = members.Select(m => new ListMemberInfo
        {
            Id = m.Id.ToString(),
            Name = m.User.Name,
            Surname = m.User.Surname,
            Photo = m.User.ProfilePicture ?? string.Empty,
            CanAddDevices = m.HasPermission(new HomePermission(HomePermission.AddDevice)),
            CanListDevices =
                m.HasPermission(new HomePermission(HomePermission.GetDevices)),
            ShouldBeNotified =
                m.HasPermission(new HomePermission(HomePermission.GetNotifications))
        }).ToList();
        return new GetMembersResponse { Members = memberInfos };
    }

    [HttpGet("{homesId}/devices")]
    [AuthorizationFilter(SystemPermission.GetDevices)]
    [HomeAuthorizationFilter(HomePermission.GetDevices)]
    public GetDevicesResponse GetDevices([FromRoute] string homesId, [FromQuery] string? roomId = null)
    {
        IEnumerable<OwnedDevice> devices = homeOwnerService.GetHomeDevices(homesId, roomId);
        var deviceInfos = devices.Select(ListOwnedDeviceInfo.FromOwnedDevice).ToList();
        return new GetDevicesResponse { Devices = deviceInfos };
    }

    [HttpPost("{homesId}/devices")]
    [AuthorizationFilter(SystemPermission.AddDevice)]
    [HomeAuthorizationFilter(HomePermission.AddDevice)]
    public AddDevicesResponse AddDevices([FromRoute] string homesId, [FromBody] AddDevicesRequest request)
    {
        AddDevicesArgs addDevicesArgs = FromRequestToAddDevicesArgs(homesId, request);
        homeOwnerService.AddDeviceToHome(addDevicesArgs);
        return new AddDevicesResponse { HomeId = homesId, DeviceIds = request.DeviceIds! };
    }

    private static AddDevicesArgs FromRequestToAddDevicesArgs(string homesId, AddDevicesRequest request)
    {
        var addDevicesArgs = new AddDevicesArgs { HomeId = homesId, DeviceIds = request.DeviceIds ?? [] };
        return addDevicesArgs;
    }

    [HttpPost("{homesId}/rooms")]
    [AuthorizationFilter(SystemPermission.CreateRoom)]
    [HomeAuthorizationFilter(HomePermission.CreateRoom)]
    public CreateRoomResponse CreateRoom([FromRoute] string homesId, [FromBody] CreateRoomRequest request)
    {
        var room = homeOwnerService.CreateRoom(homesId, request.Name ?? string.Empty);
        return new CreateRoomResponse { RoomId = room.Id.ToString() };
    }

    [HttpGet("{homesId}/rooms")]
    public GetRoomsResponse GetRooms([FromRoute] string homesId)
    {
        IEnumerable<Room> rooms = homeOwnerService.GetRoomsByHomeId(homesId);
        var roomInfos = rooms.Select(ListRoomInfo.FromRoom).ToList();
        return new GetRoomsResponse { Rooms = roomInfos };
    }
}
