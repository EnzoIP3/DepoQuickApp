namespace BusinessLogic.Devices.Importer;

public interface IDeviceImporter
{
    List<DeviceArgs> ImportDevices(Dictionary<string, string> parameters);
    List<string> GetParams();
}
