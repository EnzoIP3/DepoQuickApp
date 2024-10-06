namespace HomeConnect.WebApi.Controllers.Home.Models;

public record AddDevicesResponse
{
    public string HomeId { get; set; } = null!;
    public List<string> DeviceIds { get; set; } = null!;
}
