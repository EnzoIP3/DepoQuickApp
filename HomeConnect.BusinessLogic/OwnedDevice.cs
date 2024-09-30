namespace BusinessLogic;

public class OwnedDevice
{
    public Home Home { get; init; }
    public Device Device { get; init; }
    public Guid HardwareId { get; } = Guid.NewGuid();

    public OwnedDevice(Home home, Device device)
    {
        Home = home;
        Device = device;
    }
}
