using BusinessLogic.HomeOwners.Services;
using BusinessLogic.Roles.Entities;
using HomeConnect.WebApi.Controllers.Rooms.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("rooms")]
public class RoomController(IHomeOwnerService homeOwnerService) : ControllerBase
{
    [HttpPost]
    [AuthenticationFilter]
    [AuthorizationFilter(SystemPermission.CreateRoom)]
    public CreateRoomResponse CreateRoom([FromBody] AddRoomArgs args)
    {
        var room = homeOwnerService.CreateRoom(args.HomeId, args.Name);
        return new CreateRoomResponse { RoomId = room.Id.ToString() };
    }

    [HttpPost("{roomId}/devices")]
    [AuthenticationFilter]
    [AuthorizationFilter(SystemPermission.AddDeviceToRoom)]
    public AddOwnedDeviceToRoomResponse AddOwnedDeviceToRoom([FromRoute] string roomId, [FromBody] AddOwnedDeviceToRoomRequest request)
    {
        Guid hardwareId = homeOwnerService.AddOwnedDeviceToRoom(roomId, request.DeviceId);
        return new AddOwnedDeviceToRoomResponse { DeviceId = hardwareId.ToString(), RoomId = roomId };
    }
}
