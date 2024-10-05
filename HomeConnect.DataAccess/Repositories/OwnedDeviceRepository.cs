using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.HomeOwners.Entities;

namespace HomeConnect.DataAccess.Repositories;

public class OwnedDeviceRepository : IOwnedDeviceRepository
{
    private readonly Context _context;

    public OwnedDeviceRepository(Context context)
    {
        _context = context;
    }

    public void Add(OwnedDevice ownedDevice)
    {
        _context.OwnedDevices.Add(ownedDevice);
        _context.SaveChanges();
    }

    public IEnumerable<OwnedDevice> GetOwnedDevicesByHome(Home home)
    {
        return _context.OwnedDevices.Where(od => od.Home == home);
    }
}
