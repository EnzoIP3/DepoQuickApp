namespace HomeConnect.WebApi.Controllers.Homes.Models;

public record AddDevicesRequest
{
    public List<string>? DeviceIds { get; set; } = null!;
}
