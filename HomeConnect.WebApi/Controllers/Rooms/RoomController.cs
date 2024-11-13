using BusinessLogic.HomeOwners.Services;
using HomeConnect.WebApi.Controllers.Rooms.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("rooms")]
public class RoomController : ControllerBase
{
    [HttpPost]
    public CreateRoomResponse CreateRoom([FromBody] AddRoomArgs args)
    {
        throw new NotImplementedException();
    }
}
