using System.ComponentModel.DataAnnotations;
using BusinessLogic.HomeOwners.Entities;

namespace BusinessLogic.Devices.Entities;

public class LampOwnedDevice : OwnedDevice
{
    public LampOwnedDevice()
    {
    }

    public LampOwnedDevice(Home home, Device device)
    {
        Home = home;
        Device = device;
    }

    public bool State { get; set; } = false;
}
