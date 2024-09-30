namespace BusinessLogic;

public interface IDeviceRepository
{
    Device Get(Guid deviceId);
}
