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
        PagedData<Device> devices = DeviceRepository.GetPaged(parameters);
        return devices;
    }

    public bool ToggleDevice(string hardwareId)
    {
        EnsureHardwareIdIsValid(hardwareId);
        EnsureOwnedDeviceExists(hardwareId);
        OwnedDevice ownedDevice = OwnedDeviceRepository.GetByHardwareId(Guid.Parse(hardwareId));
        ownedDevice.Connected = !ownedDevice.Connected;
        OwnedDeviceRepository.Update(ownedDevice);
        return ownedDevice.Connected;
    }

    public IEnumerable<string> GetAllDeviceTypes()
    {
        return Enum.GetNames(typeof(DeviceType));
    }

    public bool IsConnected(string hardwareId)
    {
        EnsureHardwareIdIsValid(hardwareId);
        EnsureOwnedDeviceExists(hardwareId);
        OwnedDevice ownedDevice = OwnedDeviceRepository.GetByHardwareId(Guid.Parse(hardwareId));
        return ownedDevice.Connected;
    }

    public void TurnLamp(string hardwareId, bool state)
    {
        EnsureOwnedDeviceExists(hardwareId);
    }

    private void EnsureOwnedDeviceExists(string hardwareId)
    {
        if (!OwnedDeviceRepository.Exists(Guid.Parse(hardwareId)))
        {
            throw new KeyNotFoundException("The device is not registered in this home.");
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
