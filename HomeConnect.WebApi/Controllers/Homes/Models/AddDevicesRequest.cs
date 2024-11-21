using BusinessLogic.HomeOwners.Models;

namespace HomeConnect.WebApi.Controllers.Homes.Models;

public record AddDevicesRequest
{
    public List<string>? DeviceIds { get; set; } = null!;

    public AddDevicesArgs ToArgs(string homeId)
    {
        return new AddDevicesArgs { HomeId = homeId, DeviceIds = DeviceIds ?? [] };
    }
}
