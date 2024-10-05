using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Repositories;
using HomeConnect.WebApi.Controllers.Devices.Models;

namespace BusinessLogic.Devices.Services;

public class DeviceService : IDeviceService
{
    private IDeviceRepository DeviceRepository { get; init; }
    private IOwnedDeviceRepository OwnedDeviceRepository { get; init; }

    public DeviceService(IDeviceRepository deviceRepository, IOwnedDeviceRepository ownedDeviceRepository)
    {
        DeviceRepository = deviceRepository;
        OwnedDeviceRepository = ownedDeviceRepository;
    }

    public PagedData<Device> GetDevices(GetDeviceArgs parameters)
    {
        parameters.Page ??= 1;
        parameters.PageSize ??= 10;
        var devices = DeviceRepository.GetDevices((int)parameters.Page, (int)parameters.PageSize,
            parameters.DeviceNameFilter, parameters.ModelNumberFilter, parameters.BusinessNameFilter,
            parameters.DeviceTypeFilter);
        return devices;
    }

    public bool Toggle(string hardwareId)
    {
        EnsureHardwareIdIsValid(hardwareId);
        EnsureOwnedDeviceExists(hardwareId);
        var connectionState = OwnedDeviceRepository.ToggleConnection(hardwareId);
        return connectionState;
    }

    public IEnumerable<string> GetAllDeviceTypes()
    {
        throw new NotImplementedException();
    }

    private void EnsureOwnedDeviceExists(string hardwareId)
    {
        if (!OwnedDeviceRepository.Exists(hardwareId))
        {
            throw new ArgumentException("Owned device does not exist");
        }
    }

    private void EnsureHardwareIdIsValid(string hardwareId)
    {
        if (string.IsNullOrWhiteSpace(hardwareId) || !Guid.TryParse(hardwareId, out _))
        {
            throw new ArgumentException("Hardware ID is invalid");
        }
    }
}
