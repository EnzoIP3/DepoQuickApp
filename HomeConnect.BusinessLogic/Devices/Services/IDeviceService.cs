using BusinessLogic;
using BusinessLogic.Devices.Entities;

public interface IDeviceService
    {
        PagedData<Device> GetDevices(int? currentPage, int? pageSize, string? deviceNameFilter, int? modelNumberFilter, string? businessNameFilter, string? deviceTypeFilter);
    }
