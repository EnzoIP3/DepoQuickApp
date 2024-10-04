using BusinessLogic.Devices.Entities;

namespace BusinessLogic.Devices.Repositories;

public interface IDeviceRepository
{
    Device Get(Guid deviceId);
    void Add(Device device);
    void EnsureDeviceDoesNotExist(Device device);
}
