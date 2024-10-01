namespace BusinessLogic;

public interface IOwnedDeviceRepository
{
    void Add(OwnedDevice ownedDevice);
    IEnumerable<OwnedDevice> GetOwnedDevicesByHome(Home home);
}
