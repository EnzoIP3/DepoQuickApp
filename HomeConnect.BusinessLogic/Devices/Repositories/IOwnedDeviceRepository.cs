using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Entities;

namespace BusinessLogic.Devices.Repositories;

public interface IOwnedDeviceRepository
{
    void Add(OwnedDevice ownedDevice);
    IEnumerable<OwnedDevice> GetOwnedDevicesByHome(Home home);
    bool ToggleConnection(string hardwareId);
    OwnedDevice GetByHardwareId(string hardwareId);
    bool Exists(string hardwareId);
    bool IsConnected(string hardwareId);
}
