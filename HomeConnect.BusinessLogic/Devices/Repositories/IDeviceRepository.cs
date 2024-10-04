using BusinessLogic.Devices.Entities;

namespace BusinessLogic.Devices.Repositories;

public interface IDeviceRepository
{
    Entities.Device Get(Guid deviceId);
    void Add(Entities.Device device);
    void EnsureDeviceDoesNotExist(Entities.Device device);
    PagedData<Device> GetDevices(int currentPage, int? pageSize, string? deviceNameFilter, int? modelNumberFilter, string? businessNameFilter, string? deviceTypeFilter);
}
