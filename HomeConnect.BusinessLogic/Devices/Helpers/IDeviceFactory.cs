using BusinessLogic.Users.Entities;
using DeviceImporter.Models;

namespace BusinessLogic.Devices.Helpers;

public interface IDeviceFactory
{
    void CreateDevice(User user, DeviceArgs deviceArg);
}
