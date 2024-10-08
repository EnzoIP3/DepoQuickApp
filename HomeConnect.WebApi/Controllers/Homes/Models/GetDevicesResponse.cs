namespace HomeConnect.WebApi.Controllers.Homes.Models;

public record GetDevicesResponse
{
    public List<ListDeviceInfo> Devices { get; set; } = null!;
}
