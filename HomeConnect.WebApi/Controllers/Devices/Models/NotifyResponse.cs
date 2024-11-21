namespace HomeConnect.WebApi.Controllers.Devices.Models;

public sealed record NotifyResponse
{
    public string HardwareId { get; set; } = null!;
}
