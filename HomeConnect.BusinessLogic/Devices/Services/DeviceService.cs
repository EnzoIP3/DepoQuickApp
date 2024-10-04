using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Repositories;

namespace BusinessLogic.Devices.Services;

public class DeviceService : IDeviceService
{
    private IDeviceRepository DeviceRepository { get; init; }

    public DeviceService(IDeviceRepository deviceRepository)
    {
        DeviceRepository = deviceRepository;
    }

    public PagedData<Device> GetDevices(int? currentPage = null, int? pageSize = null, string deviceNameFilter = null, int? modelNumberFilter = null, string businessNameFilter = null, string deviceTypeFilter = null)
    {
        currentPage ??= 1;
        pageSize ??= 10;
        var devices = DeviceRepository.GetDevices((int)currentPage, (int)pageSize, deviceNameFilter, modelNumberFilter, businessNameFilter, deviceTypeFilter);
        return new PagedData<Device>
        {
            Data = devices.Data,
            Page = devices.Page,
            PageSize = devices.PageSize,
            TotalPages = devices.TotalPages
        };
    }
}
