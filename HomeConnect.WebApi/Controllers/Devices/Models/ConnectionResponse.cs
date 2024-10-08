namespace HomeConnect.WebApi.Controllers.Devices.Models;

public record ConnectionResponse
{
    public bool Connected { get; set; }
    public string HardwareId { get; set; } = null!;
}
