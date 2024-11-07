namespace BusinessLogic.Devices.Services;

public interface IDeviceImporter
{
    List<DeviceArgs> ImportDevices(string route);
}
