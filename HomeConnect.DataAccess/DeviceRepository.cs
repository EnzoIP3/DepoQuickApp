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
        _context.Devices.Add(device);
        _context.SaveChanges();
    }
}
