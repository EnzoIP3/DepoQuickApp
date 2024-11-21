namespace HomeConnect.WebApi.Controllers.Rooms.Models;

public sealed class AddOwnedDeviceToRoomResponse
{
    public string DeviceId { get; set; } = null!;
    public string RoomId { get; set; } = null!;
}
