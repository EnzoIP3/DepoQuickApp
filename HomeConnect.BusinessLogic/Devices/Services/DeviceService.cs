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

    public bool TurnDevice(string hardwareId, bool state)
    {
        EnsureHardwareIdIsValid(hardwareId);
        EnsureOwnedDeviceExists(hardwareId);
        OwnedDevice ownedDevice = OwnedDeviceRepository.GetByHardwareId(Guid.Parse(hardwareId));
        ownedDevice.Connected = state;
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
        SendLampNotification(hardwareId, state, args);
        OwnedDeviceRepository.UpdateLampState(Guid.Parse(hardwareId), state);
    }

    private void SendLampNotification(string hardwareId, bool state, NotificationArgs args)
    {
        if (OwnedDeviceRepository.GetLampState(Guid.Parse(hardwareId)) != state)
        {
            NotificationService.Notify(args, this);
        }
    }

    public void UpdateSensorState(string hardwareId, bool state, NotificationArgs args)
    {
        EnsureHardwareIdIsValid(hardwareId);
        EnsureOwnedDeviceExists(hardwareId);
        SendSensorNotification(hardwareId, state, args);
        OwnedDeviceRepository.UpdateSensorState(Guid.Parse(hardwareId), state);
    }

    public Camera GetCameraById(string cameraId)
    {
        EnsureIdFormatIsValid(cameraId);
        EnsureDeviceExists(cameraId);
        EnsureDeviceIsACamera(cameraId);
        return null;
    }

    private void EnsureDeviceIsACamera(string cameraId)
    {
        if (DeviceRepository.Get(Guid.Parse(cameraId)).Type != DeviceType.Camera)
        {
            throw new InvalidOperationException("Device is not a camera.");
        }
    }

    private void EnsureDeviceExists(string cameraId)
    {
        if (!DeviceRepository.Exists(Guid.Parse(cameraId)))
        {
            throw new InvalidOperationException("Device not found.");
        }
    }

    private void EnsureIdFormatIsValid(string cameraId)
    {
        if (string.IsNullOrWhiteSpace(cameraId) || !Guid.TryParse(cameraId, out _))
        {
            throw new ArgumentException("Camera ID format is invalid.");
        }
    }

    private void SendSensorNotification(string hardwareId, bool state, NotificationArgs args)
    {
        if (OwnedDeviceRepository.GetSensorState(Guid.Parse(hardwareId)) != state)
        {
            NotificationService.Notify(args, this);
        }
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
