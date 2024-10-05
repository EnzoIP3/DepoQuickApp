using BusinessLogic;
using BusinessLogic.Devices.Entities;
using HomeConnect.WebApi.Controllers.Devices.Models;

public interface IDeviceService
{
    PagedData<Device> GetDevices(GetDeviceArgs parameters);
}
