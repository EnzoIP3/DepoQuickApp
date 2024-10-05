using BusinessLogic;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;

public interface IDeviceService
    {
        PagedData<Device> GetDevices(int? currentPage, int? pageSize, string? deviceNameFilter, int? modelNumberFilter, string? businessNameFilter, string? deviceTypeFilter);
    }
