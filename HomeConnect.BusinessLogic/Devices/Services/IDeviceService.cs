using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;

namespace BusinessLogic.Devices.Services;

public interface IDeviceService
{
    PagedData<Device> GetDevices(GetDeviceArgs parameters);
    bool Toggle(string hardwareId);
    IEnumerable<string> GetAllDeviceTypes();
    bool IsConnected(string hardwareId);
}
