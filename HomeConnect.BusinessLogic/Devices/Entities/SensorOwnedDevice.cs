using BusinessLogic.Devices.Models;
using BusinessLogic.HomeOwners.Entities;

namespace BusinessLogic.Devices.Entities;

public class SensorOwnedDevice : OwnedDevice
{
    public SensorOwnedDevice()
    {
    }

    public SensorOwnedDevice(Home home, Device device)
        : base(home, device)
    {
        IsOpen = false;
    }

    public bool IsOpen { get; set; }

    public override OwnedDeviceDto ToOwnedDeviceDto()
    {
        var dto = base.ToOwnedDeviceDto();
        dto.IsOpen = IsOpen;
        return dto;
    }
}
