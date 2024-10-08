using HomeConnect.WebApi.Controllers.Homes.Models;

namespace HomeConnect.WebApi.Controllers.Devices.Models;

public record GetDevicesResponse
{
    public List<ListDeviceInfo> Devices { get; set; } = [];
    public Pagination Pagination { get; set; } = new();
}
