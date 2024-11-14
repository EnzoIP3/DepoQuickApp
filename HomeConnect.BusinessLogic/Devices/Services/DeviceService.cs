using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.HomeOwners.Repositories;
using BusinessLogic.Notifications.Models;
using BusinessLogic.Notifications.Services;

namespace BusinessLogic.Devices.Services;

public class DeviceService : IDeviceService
{
    public DeviceService(IDeviceRepository deviceRepository, IOwnedDeviceRepository ownedDeviceRepository,
        INotificationService notificationService, IHomeRepository homeRepository)
    {
        HomeRepository = homeRepository;
        DeviceRepository = deviceRepository;
        OwnedDeviceRepository = ownedDeviceRepository;
        NotificationService = notificationService;
    }

    private IDeviceRepository DeviceRepository { get; }
    private IOwnedDeviceRepository OwnedDeviceRepository { get; }
    private INotificationService NotificationService { get; }
    private IHomeRepository HomeRepository { get; }

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

    public void MoveDevice(string sourceRoomId, string targetRoomId, string ownedDeviceId)
    {
        var sourceRoom = HomeRepository.GetRoomById(Guid.Parse(sourceRoomId));
        var targetRoom = HomeRepository.GetRoomById(Guid.Parse(targetRoomId));

        var ownedDevice = sourceRoom.OwnedDevices.FirstOrDefault(d => d.HardwareId == Guid.Parse(ownedDeviceId));
        if (ownedDevice == null)
        {
            throw new ArgumentException("Device not found in source room.");
        }

        sourceRoom.OwnedDevices.Remove(ownedDevice);
        targetRoom.OwnedDevices.Add(ownedDevice);
        ownedDevice.Room = targetRoom;

        HomeRepository.UpdateRoom(sourceRoom);
        HomeRepository.UpdateRoom(targetRoom);
        DeviceRepository.UpdateDevice(ownedDevice);
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
