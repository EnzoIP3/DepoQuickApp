namespace BusinessLogic.Devices.Repositories;

public interface IDeviceRepository
{
    Entities.Device Get(Guid deviceId);
    void Add(Entities.Device device);
    void EnsureDeviceDoesNotExist(Entities.Device device);
}
