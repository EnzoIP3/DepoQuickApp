namespace HomeConnect.WebApi.Controllers.Home.Models;

public record GetDevicesResponse
{
    public List<ListDeviceInfo> Devices { get; set; } = null!;
}
