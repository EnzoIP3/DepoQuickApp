namespace HomeConnect.WebApi.Controllers.Home.Models;

public record AddDevicesRequest
{
    public List<string>? DeviceIds { get; set; } = null!;
}
