using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;

namespace BusinessLogic.Devices.Services;

public interface IDeviceService
{
    PagedData<Device> GetDevices(GetDevicesArgs parameters);
    bool ToggleDevice(string hardwareId);
    IEnumerable<string> GetAllDeviceTypes();
    bool IsConnected(string hardwareId);
    void TurnLamp(string hardwareId, bool state);
}
