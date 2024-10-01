namespace BusinessLogic;

public interface IDeviceRepository
{
    Device Get(Guid deviceId);
    void Add(Device device);
    void EnsureDeviceDoesNotExist(Device device);
}
