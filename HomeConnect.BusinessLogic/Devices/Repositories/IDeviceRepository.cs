using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;

namespace BusinessLogic.Devices.Repositories;

public interface IDeviceRepository
{
    PagedData<Device> GetDevices(GetDevicesArgs args);
    Device Get(Guid deviceId);
    void Add(Device device);
    bool ExistsByModelNumber(int modelNumber);
}
