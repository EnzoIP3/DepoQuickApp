using BusinessLogic;

namespace HomeConnect.DataAccess;

public class DeviceRepository
{
    private readonly Context _context;

    public DeviceRepository(Context context)
    {
        _context = context;
    }

    public void Add(Device device)
    {
        if (_context.Devices.Any(d => d.ModelNumber == device.ModelNumber))
        {
            throw new ArgumentException("Device already exists.");
        }

        _context.Devices.Add(device);
        _context.SaveChanges();
    }
}
