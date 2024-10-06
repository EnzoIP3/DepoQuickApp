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

    public bool ToggleConnection(string hardwareId)
    {
        OwnedDevice ownedDevice = _context.OwnedDevices.FirstOrDefault(od => od.HardwareId == Guid.Parse(hardwareId));
        var desiredState = !ownedDevice.Device.ConnectionState;
        ownedDevice.Device.ConnectionState = desiredState;
        _context.SaveChanges();
        return desiredState;
    }

    public OwnedDevice GetByHardwareId(string hardwareId)
    {
        return _context.OwnedDevices.FirstOrDefault(od => od.HardwareId == Guid.Parse(hardwareId));
    }

    public bool Exists(string hardwareId)
    {
        return _context.OwnedDevices.Any(od => od.HardwareId == Guid.Parse(hardwareId));
    }

    public bool IsConnected(string hardwareId)
    {
        return _context.OwnedDevices.First(od => od.HardwareId == Guid.Parse(hardwareId)).Device.ConnectionState;
    }
}
