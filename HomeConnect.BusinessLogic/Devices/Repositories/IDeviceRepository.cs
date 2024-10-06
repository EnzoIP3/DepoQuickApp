using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;

namespace BusinessLogic.Devices.Repositories;

public interface IDeviceRepository
{
    PagedData<Device> GetDevices(GetDeviceArgs args);
    Device Get(Guid deviceId);
    void Add(Device device);
    void EnsureDeviceDoesNotExist(Device device);
}
