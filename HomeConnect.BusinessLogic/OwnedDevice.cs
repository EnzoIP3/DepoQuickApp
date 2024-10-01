namespace BusinessLogic;

public class OwnedDevice
{
    public Home Home { get; init; } = null!;
    public Device Device { get; init; } = null!;
    public Guid HardwareId { get; } = Guid.NewGuid();
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
