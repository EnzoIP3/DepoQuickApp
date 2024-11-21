using BusinessLogic.HomeOwners.Models;
using BusinessLogic.Users.Entities;

namespace HomeConnect.WebApi.Controllers.Devices.Models;

public class NameDeviceRequest
{
    public string? NewName { get; set; }

    public NameDeviceArgs ToNameDeviceArgs(User user, string hardwareId)
    {
        return new NameDeviceArgs()
        {
            HardwareId = hardwareId, NewName = NewName ?? string.Empty, OwnerId = user.Id
        };
    }
}
