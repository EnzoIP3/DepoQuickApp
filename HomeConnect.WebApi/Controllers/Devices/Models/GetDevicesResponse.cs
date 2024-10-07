using HomeConnect.WebApi.Controllers.Home.Models;

namespace HomeConnect.WebApi.Controllers.Device.Models;

public record GetDevicesResponse
{
    public List<ListDeviceInfo> Devices { get; set; } = [];
    public Pagination Pagination { get; set; } = new();
}
