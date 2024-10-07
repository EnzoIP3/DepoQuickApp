using System.ComponentModel.DataAnnotations;
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
    public bool Connected { get; } = true;
}
