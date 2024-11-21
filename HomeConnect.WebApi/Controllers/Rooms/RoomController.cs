using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Services;
using BusinessLogic.Roles.Entities;
using HomeConnect.WebApi.Controllers.Rooms.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Rooms;

[ApiController]
[Route("rooms")]
public class RoomController : ControllerBase
{
    private readonly IHomeOwnerService _homeOwnerService;

    public RoomController(IHomeOwnerService homeOwnerService)
    {
        _homeOwnerService = homeOwnerService;
    }

    [HttpPost("{roomId}/devices")]
    [AuthenticationFilter]
    [AuthorizationFilter(SystemPermission.AddDeviceToRoom)]
    [HomeAuthorizationFilter(HomePermission.AddDeviceToRoom)]
    public AddOwnedDeviceToRoomResponse AddOwnedDeviceToRoom([FromRoute] string roomId,
        [FromBody] AddOwnedDeviceToRoomRequest request)
    {
        Guid hardwareId = _homeOwnerService.AddOwnedDeviceToRoom(roomId, request.DeviceId ?? string.Empty);
        return new AddOwnedDeviceToRoomResponse { DeviceId = hardwareId.ToString(), RoomId = roomId };
    }
}
