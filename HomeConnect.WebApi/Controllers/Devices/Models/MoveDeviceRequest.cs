namespace HomeConnect.WebApi.Controllers.Devices.Models;

public sealed record MoveDeviceRequest
{
    public string? TargetRoomId { get; set; }
}
