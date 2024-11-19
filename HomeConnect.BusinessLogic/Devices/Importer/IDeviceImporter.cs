namespace BusinessLogic.Devices.Importer;

public interface IDeviceImporter
{
    List<DeviceArgs> ImportDevices(string route);
    List<string> GetParams();
}
