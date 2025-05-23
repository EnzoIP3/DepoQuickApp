using System.ComponentModel.DataAnnotations;
using BusinessLogic.Devices.Models;
using BusinessLogic.HomeOwners.Entities;

namespace BusinessLogic.Devices.Entities;

public class OwnedDevice
{
    public OwnedDevice()
    {
    }

    public OwnedDevice(Home home, Device device)
    {
        Home = home;
        Device = device;
    }

    [Key]
    public Guid HardwareId { get; set; } = Guid.NewGuid();

    public Home Home { get; init; } = null!;
    public Device Device { get; init; } = null!;
    public bool Connected { get; set; } = true;
    public Room? Room { get; set; } = null!;
    public string? Name { get; set; } = null!;

    public virtual OwnedDeviceDto ToOwnedDeviceDto()
    {
        return new OwnedDeviceDto
        {
            HardwareId = HardwareId.ToString(),
            Name = Name,
            BusinessName = Device.Business.Name,
            Type = Device.Type.ToString(),
            ModelNumber = Device.ModelNumber,
            Photo = Device.MainPhoto,
            RoomId = Room?.Id.ToString(),
            SecondaryPhotos = Device.SecondaryPhotos,
            Description = Device.Description
        };
    }
}
