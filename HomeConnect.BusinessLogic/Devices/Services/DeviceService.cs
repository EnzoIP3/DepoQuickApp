using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;
using BusinessLogic.Devices.Repositories;

namespace BusinessLogic.Devices.Services;

public class DeviceService : IDeviceService
{
    public DeviceService(IDeviceRepository deviceRepository, IOwnedDeviceRepository ownedDeviceRepository)
    {
        DeviceRepository = deviceRepository;
        OwnedDeviceRepository = ownedDeviceRepository;
    }

    private IDeviceRepository DeviceRepository { get; }
    private IOwnedDeviceRepository OwnedDeviceRepository { get; }

    public PagedData<Device> GetDevices(GetDevicesArgs parameters)
    {
        parameters.Page ??= 1;
        parameters.PageSize ??= 10;
        PagedData<Device> devices = DeviceRepository.GetDevices(parameters);
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
        return Enum.GetNames(typeof(DeviceType));
    }

    public bool IsConnected(string hardwareId)
    {
        EnsureHardwareIdIsValid(hardwareId);
        EnsureOwnedDeviceExists(hardwareId);
        return OwnedDeviceRepository.IsConnected(hardwareId);
    }

    private void EnsureOwnedDeviceExists(string hardwareId)
    {
        if (!OwnedDeviceRepository.Exists(hardwareId))
        {
            throw new ArgumentException("Owned device does not exist.");
        }
    }

    private void EnsureHardwareIdIsValid(string hardwareId)
    {
        if (string.IsNullOrWhiteSpace(hardwareId) || !Guid.TryParse(hardwareId, out _))
        {
            throw new ArgumentException("Hardware ID is invalid.");
        }
    }
}
