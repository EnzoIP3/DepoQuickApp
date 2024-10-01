using System.ComponentModel.DataAnnotations;

namespace BusinessLogic;

public class OwnedDevice
{
    [Key]
    public Guid HardwareId { get; set; } = Guid.NewGuid();
    public Home Home { get; init; } = null!;
    public Device Device { get; init; } = null!;
    public bool Connected { get; } = true;

    public OwnedDevice()
    {
    }

    public OwnedDevice(Home home, Device device)
    {
        Home = home;
        Device = device;
    }
}
