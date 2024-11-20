namespace HomeConnect.WebApi.Controllers.Homes.Models;

public record GetHomeDevicesResponse
{
    public List<ListOwnedDeviceInfo> Devices { get; set; } = null!;
}
