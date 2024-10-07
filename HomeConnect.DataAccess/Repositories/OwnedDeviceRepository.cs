using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.HomeOwners.Entities;
using Microsoft.EntityFrameworkCore;

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
        return _context.OwnedDevices.Include(od => od.Device).ThenInclude(d => d.Business).Where(od => od.Home == home);
    }

    public OwnedDevice GetByHardwareId(Guid hardwareId)
    {
        return _context.OwnedDevices.First(od => od.HardwareId == hardwareId);
    }

    public bool Exists(Guid hardwareId)
    {
        return _context.OwnedDevices.Any(od => od.HardwareId == hardwareId);
    }
}
