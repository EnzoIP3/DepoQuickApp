using BusinessLogic.HomeOwners.Models;

namespace HomeConnect.WebApi.Controllers.Homes.Models;

public sealed record AddDevicesRequest
{
    public List<string>? DeviceIds { get; set; } = null!;

    public AddDevicesArgs ToArgs(string homeId)
    {
        return new AddDevicesArgs { HomeId = homeId, DeviceIds = DeviceIds ?? [] };
    }
}
