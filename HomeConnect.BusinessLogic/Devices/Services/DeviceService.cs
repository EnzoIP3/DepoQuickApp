using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Repositories;
using HomeConnect.WebApi.Controllers.Devices.Models;

namespace BusinessLogic.Devices.Services;

public class DeviceService : IDeviceService
{
    private IDeviceRepository DeviceRepository { get; init; }

    public DeviceService(IDeviceRepository deviceRepository)
    {
        DeviceRepository = deviceRepository;
    }

    public PagedData<Device> GetDevices(GetDeviceArgs parameters)
    {
        parameters.Page ??= 1;
        parameters.PageSize ??= 10;
        var devices = DeviceRepository.GetDevices((int)parameters.Page, (int)parameters.PageSize, parameters.DeviceNameFilter, parameters.ModelNumberFilter, parameters.BusinessNameFilter, parameters.DeviceTypeFilter);
        return devices;
    }
}
