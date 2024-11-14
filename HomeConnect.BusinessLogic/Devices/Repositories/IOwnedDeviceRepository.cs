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
    void UpdateLampState(Guid hardwareId, bool state);
    void UpdateSensorState(Guid hardwareId, bool state);
    bool GetLampState(Guid hardwareId);
    bool GetSensorState(Guid hardwareId);
    void Rename(OwnedDevice ownedDevice, string newName);
}
