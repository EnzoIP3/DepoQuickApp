using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;
using BusinessLogic.Devices.Repositories;

namespace BusinessLogic.Devices.Services;

public class DeviceService : IDeviceService
{
    private IDeviceRepository DeviceRepository { get; init; }

    public DeviceService(IDeviceRepository deviceRepository)
    {
        DeviceRepository = deviceRepository;
    }

    public PagedData<GetDevicesArgs> GetDevices(int? currentPage = null, int? pageSize = null, string deviceNameFilter = null, int? modelNumberFilter = null, string businessNameFilter = null, string deviceTypeFilter = null)
    {
        currentPage ??= 1;
        pageSize ??= 10;
        var devices = DeviceRepository.GetDevices((int)currentPage, (int)pageSize, deviceNameFilter, modelNumberFilter, businessNameFilter, deviceTypeFilter);
        var data = devices.Data.Select(x => new GetDevicesArgs
        {
            Name = x.Name,
            ModelNumber = x.ModelNumber,
            Description = x.Description,
            MainPhoto = x.MainPhoto,
            Type = x.Type,
            BusinessName = x.Business.Name,
            OwnerEmail = x.Business.Owner.Email
        }).ToList();
        return new PagedData<GetDevicesArgs>
        {
            Data = data,
            Page = devices.Page,
            PageSize = devices.PageSize,
            TotalPages = devices.TotalPages
        };
    }
}
