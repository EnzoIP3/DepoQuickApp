using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Models;
using BusinessLogic.HomeOwners.Services;
using BusinessLogic.Roles.Entities;
using HomeConnect.WebApi.Controllers.Home.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HomeConnect.WebApi.Controllers.Home;

[ApiController]
[Route("homes")]
[AuthenticationFilter]
public class HomeController(IHomeOwnerService homeOwnerService) : ControllerBase
{
    [HttpPost]
    [AuthorizationFilter(SystemPermission.CreateHome)]
    public CreateHomeResponse CreateHome([FromBody] CreateHomeRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged];
        CreateHomeArgs createHomeArgs = HomeArgsFromRequest(request, (BusinessLogic.Users.Entities.User)userLoggedIn!);
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
            UserId = request.MemberId ?? string.Empty,
            CanAddDevices = request.CanAddDevices,
            CanListDevices = request.CanListDevices
        };
        Guid addedMemberId = homeOwnerService.AddMemberToHome(addMemberArgs);
        return new AddMemberResponse { HomeId = homesId, MemberId = addedMemberId.ToString() };
    }

    private CreateHomeArgs HomeArgsFromRequest(CreateHomeRequest request, BusinessLogic.Users.Entities.User user)
    {
        var homeArgs = new CreateHomeArgs
        {
            HomeOwnerId = user.Id.ToString(),
            Address = request.Address ?? string.Empty,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            MaxMembers = request.MaxMembers
        };
        return homeArgs;
    }

    [HttpGet("{homesId}/members")]
    [AuthorizationFilter(SystemPermission.GetMembers)]
    [HomeAuthorizationFilter(HomePermission.GetMembers)]
    public GetMembersResponse GetMembers([FromRoute] string homesId)
    {
        List<BusinessLogic.HomeOwners.Entities.Member> members = homeOwnerService.GetHomeMembers(homesId);
        var memberInfos = members.Select(m => new ListMemberInfo
        {
            Id = m.User.Id.ToString(),
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
    public GetDevicesResponse GetDevices([FromRoute] string homesId)
    {
        IEnumerable<OwnedDevice> devices = homeOwnerService.GetHomeDevices(homesId);
        var deviceInfos = devices.Select(d => new ListDeviceInfo
        {
            HardwareId = d.HardwareId.ToString(),
            Name = d.Device.Name,
            BusinessName = d.Device.Business.Name,
            Type = d.Device.Type.ToString(),
            ModelNumber = d.Device.ModelNumber,
            Photo = d.Device.MainPhoto,
            IsConnected = d.Connected
        }).ToList();
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
}
