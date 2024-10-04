using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Entities;

namespace BusinessLogic.Devices.Repositories;

public interface IOwnedDeviceRepository
{
    void Add(OwnedDevice ownedDevice);
    IEnumerable<OwnedDevice> GetOwnedDevicesByHome(Home home);
    bool ToggleConnection(string hardwareId);
}
