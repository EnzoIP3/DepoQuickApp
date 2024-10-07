namespace HomeConnect.WebApi.Controllers.Device.Models;

public record ConnectionResponse
{
    public bool Connected { get; set; }
    public string HardwareId { get; set; } = null!;
}
