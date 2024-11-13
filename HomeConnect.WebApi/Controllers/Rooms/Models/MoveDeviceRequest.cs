namespace HomeConnect.WebApi.Controllers.Rooms.Models;

public class MoveDeviceRequest
{
    public string SourceRoomId { get; set; } = null!;
    public string TargetRoomId { get; set; } = null!;
    public string DeviceId { get; set; } = null!;
}
