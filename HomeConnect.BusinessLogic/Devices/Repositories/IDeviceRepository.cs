using BusinessLogic.Devices.Entities;

namespace BusinessLogic.Devices.Repositories;

public interface IDeviceRepository
{
    PagedData<Device> GetDevices(int currentPage, int? pageSize, string? deviceNameFilter, int? modelNumberFilter, string? businessNameFilter, string? deviceTypeFilter);
    Device Get(Guid deviceId);
    void Add(Device device);
    void EnsureDeviceDoesNotExist(Device device);
}
