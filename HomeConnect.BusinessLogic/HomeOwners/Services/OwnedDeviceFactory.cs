using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Entities;

namespace BusinessLogic.HomeOwners.Services;

internal sealed class OwnedDeviceFactory
{
    public static OwnedDevice CreateOwnedDevice(Home home, Device device)
    {
        return device.Type switch
        {
            DeviceType.Lamp => new LampOwnedDevice(home, device),
            DeviceType.Sensor => new SensorOwnedDevice(home, device),
            _ => new OwnedDevice(home, device)
        };
    }
}
