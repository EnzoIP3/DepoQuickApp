using DeviceImporter.Models;

namespace DeviceImporter;

public interface IDeviceImporter
{
    List<DeviceArgs> ImportDevices(Dictionary<string, string> parameters);
    List<string> GetParams();
}
