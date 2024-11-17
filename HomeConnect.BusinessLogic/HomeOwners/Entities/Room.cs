using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Models;

namespace BusinessLogic.HomeOwners.Entities;

public class Room
{
    public Room()
    {
    }

    public Room(string name, Home home)
    {
        Name = name;
        Home = home;
    }

    private string _name = string.Empty;
    private Home _home = null!;
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name
    {
        get => _name;
        set
        {
            EnsureNameIsNotNullOrEmpty(value);
            _name = value;
        }
    }

    private static void EnsureNameIsNotNullOrEmpty(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException("Room name cannot be null or empty.");
        }
    }

    public Home Home
    {
        get => _home;
        set
        {
            _home = value ?? throw new ArgumentException("Room must have a home assigned.");
        }
    }

    public List<OwnedDevice> OwnedDevices { get; set; } = [];

    public void AddOwnedDevice(OwnedDevice device)
    {
        EnsureOwnedDeviceBelongsToTheSameHome(device);
        EnsureOwnedDeviceDoesNotAlreadyBelongToTheRoom(device);
        OwnedDevices.Add(device);
    }

    private void EnsureOwnedDeviceDoesNotAlreadyBelongToTheRoom(OwnedDevice device)
    {
        if (OwnedDevices.Any(od => od.HardwareId == device.HardwareId))
        {
            throw new ArgumentException("Device already belongs to the room.");
        }
    }

    private void EnsureOwnedDeviceBelongsToTheSameHome(OwnedDevice device)
    {
        if (device.Home.Id != Home.Id)
        {
            throw new ArgumentException("Device must belong to the same home as the room.");
        }
    }

    public void RemoveOwnedDevice(OwnedDevice device)
    {
        EnsureOwnedDeviceBelongsToRoom(device);
        OwnedDevices.Remove(device);
    }

    private void EnsureOwnedDeviceBelongsToRoom(OwnedDevice device)
    {
        if (!OwnedDevices.Contains(device))
        {
            throw new ArgumentException("Device does not belong to the room.");
        }
    }

    public OwnedDevice GetOwnedDevice(Guid hardwareId)
    {
        EnsureOwnedDeviceBelongsToRoom(hardwareId);
        return OwnedDevices.First(od => od.HardwareId == hardwareId);
    }

    private void EnsureOwnedDeviceBelongsToRoom(Guid hardwareId)
    {
        if (OwnedDevices.All(od => od.HardwareId != hardwareId))
        {
            throw new ArgumentException("Device does not belong to the room.");
        }
    }

    public RoomDto ToRoomDto()
    {
        return new RoomDto
        {
            Id = Id.ToString(),
            Name = Name,
            HomeId = Home.Id.ToString(),
            OwnedDevicesId = OwnedDevices.Select(od => od.HardwareId.ToString()).ToList()
        };
    }
}
