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
        throw new NotImplementedException();
    }
}
