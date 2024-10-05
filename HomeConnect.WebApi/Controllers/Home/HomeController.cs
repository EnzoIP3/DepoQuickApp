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
        var homeId = homeOwnerService.CreateHome(createHomeArgs);
        return new CreateHomeResponse { Id = homeId.ToString() };
    }

    [HttpPost("{homesId}/members")]
    [AuthorizationFilter(SystemPermission.AddMember)]
    public AddMemberResponse AddMember([FromRoute] string homesId, [FromBody] AddMemberRequest request)
    {
        var userLoggedIn = HttpContext.Items[Item.UserLogged];
        var addMemberArgs = ArgsFromRequest(request, homesId, (BusinessLogic.Users.Entities.User)userLoggedIn!);
        var addedMemberId = homeOwnerService.AddMemberToHome(addMemberArgs);
        return new AddMemberResponse { HomeId = homesId, MemberId = addedMemberId.ToString() };
    }

    private AddMemberArgs ArgsFromRequest(AddMemberRequest request, string homesId,
        BusinessLogic.Users.Entities.User userLoggedIn)
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

    [HttpGet("{homesId}/members")]
    [AuthorizationFilter(SystemPermission.GetMembers)]
    public GetMembersResponse GetMembers([FromRoute] string homesId, AuthorizationFilterContext context)
    {
        var userLoggedIn = context.HttpContext.Items[Item.UserLogged];
        var members = homeOwnerService.GetHomeMembers(homesId);
        var memberInfos = members.Select(m => new ListMemberInfo
        {
            Id = m.User.Id.ToString(),
            Name = m.User.Name,
            Surname = m.User.Surname,
            Photo = m.User.ProfilePicture ?? string.Empty,
            CanAddDevices = m.HasPermission(new BusinessLogic.HomeOwners.Entities.HomePermission("canAddDevices")),
            CanListDevices =
                m.HasPermission(new BusinessLogic.HomeOwners.Entities.HomePermission("canListDevices")),
            ShouldBeNotified =
                m.HasPermission(new BusinessLogic.HomeOwners.Entities.HomePermission("shouldBeNotified"))
        }).ToList();
        return new GetMembersResponse { Members = memberInfos };
    }

    [HttpGet("{homesId}/devices")]
    [AuthorizationFilter(SystemPermission.GetDevices)]
    [HomeAuthorizationFilter(HomePermission.GetDevices)]
    public GetDevicesResponse GetDevices([FromRoute] string homesId, AuthorizationFilterContext context)
    {
        var userLoggedIn = context.HttpContext.Items[Item.UserLogged];
        var devices = homeOwnerService.GetHomeDevices(homesId);
        var deviceInfos = devices.Select(d => new ListDeviceInfo
        {
            Name = d.Device.Name,
            Type = d.Device.Type.ToString(),
            ModelNumber = d.Device.ModelNumber,
            Photo = d.Device.MainPhoto,
            IsConnected = d.Connected
        }).ToList();
        return new GetDevicesResponse { Device = deviceInfos };
    }

    [HttpPost("{homesId}/devices")]
    [AuthorizationFilter(SystemPermission.AddDevice)]
    [HomeAuthorizationFilter(HomePermission.AddDevices)]
    public AddDevicesResponse AddDevices([FromRoute] string homesId, [FromBody] AddDevicesRequest request)
    {
        AddDevicesArgs addDevicesArgs = FromRequestToAddDevicesArgs(homesId, request);
        homeOwnerService.AddDeviceToHome(addDevicesArgs);
        return new AddDevicesResponse { HomeId = homesId, DeviceIds = request.DeviceIds };
    }

    private static AddDevicesArgs FromRequestToAddDevicesArgs(string homesId, AddDevicesRequest request)
    {
        var addDevicesArgs = new AddDevicesArgs { HomeId = homesId, DeviceIds = request.DeviceIds };
        return addDevicesArgs;
    }
}
