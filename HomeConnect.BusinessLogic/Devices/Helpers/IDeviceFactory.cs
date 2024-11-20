using BusinessLogic.Devices.Importer;
using BusinessLogic.Users.Entities;

namespace BusinessLogic.Devices.Helpers;

public interface IDeviceFactory
{
    void CreateDevice(User user, DeviceArgs deviceArg);
}
