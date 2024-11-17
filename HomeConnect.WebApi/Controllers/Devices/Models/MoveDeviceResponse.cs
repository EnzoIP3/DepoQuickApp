namespace HomeConnect.WebApi.Controllers.Rooms.Models;

public class MoveDeviceResponse
{
    public string TargetRoomId { get; set; } = null!;
    public string DeviceId { get; set; } = null!;
}
