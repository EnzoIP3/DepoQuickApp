using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.HomeOwners.Repositories;

namespace BusinessLogic.Devices.Services;

public class DeviceService : IDeviceService
{
    public DeviceService(IDeviceRepository deviceRepository, IOwnedDeviceRepository ownedDeviceRepository,
        IRoomRepository roomRepository)
    {
        _deviceRepository = deviceRepository;
        _ownedDeviceRepository = ownedDeviceRepository;
        _roomRepository = roomRepository;
    }

    private readonly IDeviceRepository _deviceRepository;
    private readonly IOwnedDeviceRepository _ownedDeviceRepository;
    private readonly IRoomRepository _roomRepository;

    public PagedData<Device> GetDevices(GetDevicesArgs parameters)
    {
        EnsureDeviceTypeExists(parameters);
        parameters.Page ??= 1;
        parameters.PageSize ??= 10;
        PagedData<Device> devices = _deviceRepository.GetPaged(parameters);
        return devices;
    }

    private static void EnsureDeviceTypeExists(GetDevicesArgs parameters)
    {
        if (parameters.DeviceTypeFilter != null && !Enum.TryParse(parameters.DeviceTypeFilter, out DeviceType _))
        {
            throw new ArgumentException("That device type does not exist.");
        }
    }

    public bool TurnDevice(string hardwareId, bool state)
    {
        EnsureHardwareIdIsValid(hardwareId);
        EnsureOwnedDeviceExists(hardwareId);
        OwnedDevice ownedDevice = _ownedDeviceRepository.GetByHardwareId(Guid.Parse(hardwareId));
        ownedDevice.Connected = state;
        _ownedDeviceRepository.Update(ownedDevice);
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
        OwnedDevice ownedDevice = _ownedDeviceRepository.GetByHardwareId(Guid.Parse(hardwareId));
        return ownedDevice.Connected;
    }

    public void TurnLamp(string hardwareId, bool state)
    {
        EnsureHardwareIdIsValid(hardwareId);
        EnsureOwnedDeviceExists(hardwareId);
        EnsureDeviceIsALamp(hardwareId);
        _ownedDeviceRepository.UpdateLampState(Guid.Parse(hardwareId), state);
    }

    private void EnsureDeviceIsALamp(string hardwareId)
    {
        if (_ownedDeviceRepository.GetByHardwareId(Guid.Parse(hardwareId)).Device.Type != DeviceType.Lamp)
        {
            throw new InvalidOperationException("Device is not a lamp.");
        }
    }

    public void UpdateSensorState(string hardwareId, bool state)
    {
        EnsureHardwareIdIsValid(hardwareId);
        EnsureOwnedDeviceExists(hardwareId);
        EnsureDeviceIsASensor(hardwareId);
        _ownedDeviceRepository.UpdateSensorState(Guid.Parse(hardwareId), state);
    }

    private void EnsureDeviceIsASensor(string hardwareId)
    {
        if (_ownedDeviceRepository.GetByHardwareId(Guid.Parse(hardwareId)).Device.Type != DeviceType.Sensor)
        {
            throw new InvalidOperationException("Device is not a sensor.");
        }
    }

    public Camera GetCameraById(string cameraId)
    {
        EnsureIdFormatIsValid(cameraId);
        EnsureDeviceExists(cameraId);
        EnsureDeviceIsACamera(cameraId);
        return _deviceRepository.Get(Guid.Parse(cameraId)!) as Camera;
    }

    private void EnsureDeviceIsACamera(string cameraId)
    {
        if (_deviceRepository.Get(Guid.Parse(cameraId)).Type != DeviceType.Camera)
        {
            throw new InvalidOperationException("Device is not a camera.");
        }
    }

    private void EnsureDeviceExists(string cameraId)
    {
        if (!_deviceRepository.Exists(Guid.Parse(cameraId)))
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

    public void MoveDevice(string targetRoomId, string ownedDeviceId)
    {
        if (!_roomRepository.Exists(Guid.Parse(targetRoomId)))
        {
            throw new ArgumentException("The room where the device should be moved does not exist.");
        }

        var targetRoom = _roomRepository.Get(Guid.Parse(targetRoomId));
        var ownedDevice = _ownedDeviceRepository.GetByHardwareId(Guid.Parse(ownedDeviceId));

        if (ownedDevice.Room != null)
        {
            targetRoom.AddOwnedDevice(ownedDevice);
            var sourceRoom = _roomRepository.Get(ownedDevice.Room.Id);
            sourceRoom.RemoveOwnedDevice(ownedDevice);
            _roomRepository.Update(sourceRoom);
            _roomRepository.Update(targetRoom);
            _ownedDeviceRepository.Update(ownedDevice);
        }
        else
        {
            throw new ArgumentException("Device is not in a room.");
        }
    }

    private void EnsureOwnedDeviceExists(string hardwareId)
    {
        if (!_ownedDeviceRepository.Exists(Guid.Parse(hardwareId)))
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
