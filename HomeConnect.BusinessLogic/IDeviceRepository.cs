namespace BusinessLogic;

public interface IDeviceRepository
{
    void Add(Device device);
    void EnsureDeviceDoesNotExist(Device device);
}
