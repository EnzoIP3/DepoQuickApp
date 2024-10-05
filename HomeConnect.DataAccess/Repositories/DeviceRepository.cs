using BusinessLogic;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Repositories;

namespace HomeConnect.DataAccess.Repositories;

public class DeviceRepository : PaginatedRepositoryBase<Device>, IDeviceRepository
{
    private readonly Context _context;

    public DeviceRepository(Context context)
        : base(context)
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

    public PagedData<Device> GetDevices(int currentPage, int? pageSize, string? deviceNameFilter = null,
        int? modelNumberFilter = null, string? businessNameFilter = null, string? deviceTypeFilter = null)
    {
        var filters = new object[4];
        filters[0] = deviceNameFilter ?? string.Empty;
        filters[1] = modelNumberFilter;
        filters[2] = businessNameFilter ?? string.Empty;
        filters[3] = deviceTypeFilter ?? string.Empty;
        return GetAllPaged(currentPage, pageSize ?? 10, filters);
    }

    protected override IQueryable<Device> GetQueryable()
    {
        return _context.Devices;
    }

    protected override IQueryable<Device> ApplyFilters(IQueryable<Device> query, params object[] filters)
    {
        var deviceNameFilter = filters.Length > 0 ? filters[0] as string : null;

        if (!string.IsNullOrEmpty(deviceNameFilter))
        {
            query = query.Where(d => d.Name.Contains(deviceNameFilter));
        }

        return query;
    }
}
