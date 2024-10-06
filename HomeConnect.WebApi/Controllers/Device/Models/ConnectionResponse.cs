namespace HomeConnect.WebApi.Controllers.Device.Models;

public record ConnectionResponse
{
    public bool ConnectionState { get; set; }
    public string HardwareId { get; set; } = null!;
}
