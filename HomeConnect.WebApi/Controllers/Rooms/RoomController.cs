using BusinessLogic.HomeOwners.Services;
using HomeConnect.WebApi.Controllers.Rooms.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("rooms")]
public class RoomController(IHomeOwnerService homeOwnerService) : ControllerBase
{
    [HttpPost]
    public CreateRoomResponse CreateRoom([FromBody] AddRoomArgs args)
    {
        var room = homeOwnerService.CreateRoom(args.HomeId, args.Name);
        return new CreateRoomResponse { RoomId = room.Id.ToString() };
    }

    [HttpPost("{roomId}/devices")]
    public AddOwnedDeviceToRoomResponse AddOwnedDeviceToRoom([FromRoute] string roomId, [FromBody] AddOwnedDeviceToRoomRequest request)
    {
        Guid hardwareId = homeOwnerService.AddOwnedDeviceToRoom(roomId, request.DeviceId);
        return new AddOwnedDeviceToRoomResponse { DeviceId = hardwareId.ToString(), RoomId = roomId };
    }

    [HttpPost("{deviceId}/move")]
    public MoveDeviceResponse MoveDevice([FromRoute] string deviceId, [FromBody] MoveDeviceRequest request)
    {
        homeOwnerService.MoveDevice(request.SourceRoomId, request.TargetRoomId, deviceId);
        return new MoveDeviceResponse
        {
            SourceRoomId = request.SourceRoomId,
            TargetRoomId = request.TargetRoomId,
            DeviceId = deviceId
        };
    }
}
