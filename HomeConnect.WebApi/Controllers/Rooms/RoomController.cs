using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Services;
using BusinessLogic.Roles.Entities;
using HomeConnect.WebApi.Controllers.Rooms.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Rooms;

[ApiController]
[Route("rooms")]
public class RoomController(IHomeOwnerService homeOwnerService) : ControllerBase
{
    [HttpPost("{roomId}/devices")]
    [AuthenticationFilter]
    [AuthorizationFilter(SystemPermission.AddDeviceToRoom)]
    [HomeAuthorizationFilter(HomePermission.AddDevice)]
    public AddOwnedDeviceToRoomResponse AddOwnedDeviceToRoom([FromRoute] string roomId,
        [FromBody] AddOwnedDeviceToRoomRequest request)
    {
        Guid hardwareId = homeOwnerService.AddOwnedDeviceToRoom(roomId, request.DeviceId);
        return new AddOwnedDeviceToRoomResponse { DeviceId = hardwareId.ToString(), RoomId = roomId };
    }
}
