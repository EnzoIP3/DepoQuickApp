using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.Notifications.Models;
using BusinessLogic.Notifications.Services;

namespace BusinessLogic.Devices.Services;

public class DeviceService : IDeviceService
{
    public DeviceService(IDeviceRepository deviceRepository, IOwnedDeviceRepository ownedDeviceRepository,
        INotificationService notificationService)
    {
        DeviceRepository = deviceRepository;
        OwnedDeviceRepository = ownedDeviceRepository;
        NotificationService = notificationService;
    }

    private IDeviceRepository DeviceRepository { get; }
    private IOwnedDeviceRepository OwnedDeviceRepository { get; }
    private INotificationService NotificationService { get; }

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

    public void TurnLamp(string hardwareId, bool state, NotificationArgs args)
    {
        EnsureHardwareIdIsValid(hardwareId);
        EnsureOwnedDeviceExists(hardwareId);
        SendNotification(hardwareId, state, args);
        OwnedDeviceRepository.UpdateLampState(Guid.Parse(hardwareId), state);
    }

    private void SendNotification(string hardwareId, bool state, NotificationArgs args)
    {
        if (OwnedDeviceRepository.GetLampState(Guid.Parse(hardwareId)) == state)
        {
            return;
        }
    }

    public void UpdateSensorState(string hardwareId, bool state)
    {
        EnsureHardwareIdIsValid(hardwareId);
        EnsureOwnedDeviceExists(hardwareId);
        OwnedDeviceRepository.UpdateSensorState(Guid.Parse(hardwareId), state);
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
