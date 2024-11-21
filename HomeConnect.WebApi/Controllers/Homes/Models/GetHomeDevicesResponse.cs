using BusinessLogic.Devices.Entities;

namespace HomeConnect.WebApi.Controllers.Homes.Models;

public sealed record GetHomeDevicesResponse
{
    public List<ListOwnedDeviceInfo> Devices { get; set; } = null!;

    public static GetHomeDevicesResponse FromDevices(IEnumerable<OwnedDevice> devices)
    {
        var deviceInfos = devices.Select(ListOwnedDeviceInfo.FromOwnedDevice).ToList();
        return new GetHomeDevicesResponse { Devices = deviceInfos };
    }
}
