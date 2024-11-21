namespace HomeConnect.WebApi.Controllers.Devices.Models;

public sealed record MoveDeviceResponse
{
    public string TargetRoomId { get; set; } = null!;
    public string DeviceId { get; set; } = null!;
}
