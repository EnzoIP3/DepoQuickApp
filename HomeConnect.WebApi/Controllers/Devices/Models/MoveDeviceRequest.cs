namespace HomeConnect.WebApi.Controllers.Rooms.Models;

public class MoveDeviceRequest
{
    public string? SourceRoomId { get; set; }
    public string? TargetRoomId { get; set; }
}
