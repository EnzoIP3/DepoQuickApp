using BusinessLogic.Devices.Entities;
using HomeConnect.WebApi.Controllers.Devices.Models;

namespace BusinessLogic.Devices.Services;

public interface IDeviceService
{
    PagedData<Device> GetDevices(GetDeviceArgs parameters);
    bool Toggle(string hardwareId);
}
