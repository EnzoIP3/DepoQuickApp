using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Repositories;

namespace HomeConnect.DataAccess.Repositories;

public class DeviceRepository : IDeviceRepository
{
    private readonly Context _context;

    public DeviceRepository(Context context)
    {
        _context = context;
    }

    public Device Get(Guid deviceId)
    {
        Device? device = _context.Devices.FirstOrDefault(d => d.Id == deviceId);
        if (device == null)
        {
            throw new ArgumentException("Device does not exist");
        }

        return device;
    }

    public void Add(Device device)
    {
        EnsureDeviceDoesNotExist(device);
        _context.Devices.Add(device);
        _context.SaveChanges();
    }

    public void EnsureDeviceDoesNotExist(Device device)
    {
        if (_context.Devices.Any(d => d.ModelNumber == device.ModelNumber))
        {
            throw new ArgumentException("Device already exists.");
        }
    }
}
