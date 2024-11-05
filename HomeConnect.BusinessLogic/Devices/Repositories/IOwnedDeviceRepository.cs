using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Entities;

namespace BusinessLogic.Devices.Repositories;

public interface IOwnedDeviceRepository
{
    void Add(OwnedDevice ownedDevice);
    IEnumerable<OwnedDevice> GetOwnedDevicesByHome(Home home);
    OwnedDevice GetByHardwareId(Guid hardwareId);
    bool Exists(Guid hardwareId);
    void Update(OwnedDevice ownedDevice);
    void Rename(OwnedDevice ownedDevice, string newName);
}
