using System.ComponentModel.DataAnnotations;
using BusinessLogic.Devices.Models;
using BusinessLogic.HomeOwners.Entities;

namespace BusinessLogic.Devices.Entities;

public class LampOwnedDevice : OwnedDevice
{
    public LampOwnedDevice()
    {
    }

    public LampOwnedDevice(Home home, Device device)
        : base(home, device)
    {
        State = false;
    }

    public bool State { get; set; }

    public override OwnedDeviceDto ToOwnedDeviceDto()
    {
        var dto = base.ToOwnedDeviceDto();
        dto.State = State;
        return dto;
    }
}
