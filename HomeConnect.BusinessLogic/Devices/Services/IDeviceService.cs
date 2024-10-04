using BusinessLogic.Devices.Entities;

namespace BusinessLogic.Devices.Services;

public interface IDeviceService
{
       PagedData<Device> GetDevices(string? deviceName, string? device, string? model, string? business, int? page, int? pageSize);
}
