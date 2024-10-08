using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;

namespace BusinessLogic.Devices.Repositories;

public interface IDeviceRepository
{
    PagedData<Device> GetPaged(GetDevicesArgs args);
    Device Get(Guid deviceId);
    void Add(Device device);
    bool ExistsByModelNumber(int modelNumber);
}
