using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;
using BusinessLogic.Notifications.Models;

namespace BusinessLogic.Devices.Services;

public interface IDeviceService
{
    PagedData<Device> GetDevices(GetDevicesArgs parameters);
    bool TurnDevice(string hardwareId, bool state);
    IEnumerable<string> GetAllDeviceTypes();
    bool IsConnected(string hardwareId);
    void TurnLamp(string hardwareId, bool state, NotificationArgs args);
    void UpdateSensorState(string hardwareId, bool state, NotificationArgs args);
    public Camera GetCameraById(string cameraId);
    void MoveDevice(string targetRoomId, string deviceId);
}
