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
        return _context.OwnedDevices.Include(od => od.Home).ThenInclude(h => h.Members)
            .ThenInclude(m => m.User).Include(od => od.Home).ThenInclude(h => h.Members)
            .ThenInclude(m => m.HomePermissions).Include(od => od.Home).ThenInclude(h => h.Owner)
            .First(od => od.HardwareId == hardwareId);
    }

    public bool Exists(Guid hardwareId)
    {
        return _context.OwnedDevices.Any(od => od.HardwareId == hardwareId);
    }

    public void Update(OwnedDevice ownedDevice)
    {
        _context.OwnedDevices.Update(ownedDevice);
        _context.SaveChanges();
    }

    public void UpdateLampState(Guid hardwareId, bool state)
    {
        EnsureDeviceIsLamp(hardwareId);
        LampOwnedDevice lamp = (LampOwnedDevice)GetByHardwareId(hardwareId);
        lamp.State = state;
        Update(lamp);
    }

    public void UpdateSensorState(Guid hardwareId, bool state)
    {
        EnsureDeviceIsSensor(hardwareId);
    }

    public bool GetLampState(Guid hardwareId)
    {
        EnsureDeviceIsLamp(hardwareId);
        return ((LampOwnedDevice)GetByHardwareId(hardwareId)).State;
    }

    private void EnsureDeviceIsSensor(Guid hardwareId)
    {
        OwnedDevice ownedDevice = GetByHardwareId(hardwareId);
        if (ownedDevice.Device.Type != DeviceType.Sensor)
        {
            throw new InvalidOperationException("The device is not a sensor.");
        }
    }

    private void EnsureDeviceIsLamp(Guid hardwareId)
    {
        OwnedDevice ownedDevice = GetByHardwareId(hardwareId);
        if (ownedDevice.Device.Type != DeviceType.Lamp)
        {
            throw new InvalidOperationException("The device is not a lamp.");
        }
    }
}
