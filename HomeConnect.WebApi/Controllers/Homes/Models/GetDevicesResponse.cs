namespace HomeConnect.WebApi.Controllers.Homes.Models;

public record GetDevicesResponse
{
    public List<ListOwnedDeviceInfo> Devices { get; set; } = null!;
}
