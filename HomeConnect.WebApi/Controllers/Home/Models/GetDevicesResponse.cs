namespace HomeConnect.WebApi.Controllers.Home.Models;

public record GetDevicesResponse
{
    public List<ListDeviceInfo> Device { get; set; } = null!;
}
