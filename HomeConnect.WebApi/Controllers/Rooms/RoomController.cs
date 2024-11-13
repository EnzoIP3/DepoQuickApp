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
        throw new NotImplementedException();
    }
}
