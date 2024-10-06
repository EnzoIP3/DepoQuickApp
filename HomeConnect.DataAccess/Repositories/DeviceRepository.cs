using BusinessLogic;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Devices.Models;
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
            throw new InvalidOperationException("Device already exists.");
        }
    }

    public PagedData<Device> GetDevices(GetDeviceArgs args)
    {
        var filters = new object[4];
        filters[0] = args.DeviceNameFilter ?? string.Empty;
        filters[1] = args.ModelNumberFilter;
        filters[2] = args.BusinessNameFilter ?? string.Empty;
        filters[3] = args.DeviceTypeFilter ?? string.Empty;
        return GetAllPaged(args.Page ?? 1, args.PageSize ?? 20, filters);
    }

    protected override IQueryable<Device> GetQueryable()
    {
        return _context.Devices;
    }

    protected override IQueryable<Device> ApplyFilters(IQueryable<Device> query, params object[] filters)
    {
        var deviceNameFilter = filters.Length > 0 ? filters[0] as string : null;
        var modelNumberFilter = filters.Length > 1 ? filters[1] as int? : null;
        var businessNameFilter = filters.Length > 2 ? filters[2] as string : null;
        var deviceTypeFilter = filters.Length > 3 ? filters[3] as string : null;

        query = FilterByDeviceName(deviceNameFilter, query);
        query = FilterByModelNumber(modelNumberFilter, query);
        query = FilterByBusinessName(businessNameFilter, query);
        query = FilterByDeviceType(deviceTypeFilter, query);

        return query;
    }

    private static IQueryable<Device> FilterByDeviceName(string? deviceNameFilter, IQueryable<Device> query)
    {
        if (!string.IsNullOrEmpty(deviceNameFilter))
        {
            query = query.Where(d => d.Name == deviceNameFilter);
        }

        return query;
    }

    private static IQueryable<Device> FilterByModelNumber(int? modelNumberFilter, IQueryable<Device> query)
    {
        if (modelNumberFilter.HasValue)
        {
            query = query.Where(d => d.ModelNumber == modelNumberFilter.Value);
        }

        return query;
    }

    private static IQueryable<Device> FilterByBusinessName(string? businessNameFilter, IQueryable<Device> query)
    {
        if (!string.IsNullOrEmpty(businessNameFilter))
        {
            query = query.Where(d => d.Business.Name.Contains(businessNameFilter));
        }

        return query;
    }

    private static IQueryable<Device> FilterByDeviceType(string? deviceTypeFilter, IQueryable<Device> query)
    {
        if (!string.IsNullOrEmpty(deviceTypeFilter))
        {
            if (Enum.TryParse(deviceTypeFilter, out DeviceType deviceType))
            {
                query = query.Where(d => d.Type == deviceType);
            }
            else
            {
                throw new ArgumentException("Invalid device type filter");
            }
        }

        return query;
    }
}
