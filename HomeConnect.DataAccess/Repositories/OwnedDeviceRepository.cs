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
        return _context.OwnedDevices.Include(od => od.Room).Include(od => od.Device).ThenInclude(d => d.Business)
            .Where(od => od.Home == home);
    }

    public OwnedDevice GetByHardwareId(Guid hardwareId)
    {
        return _context.OwnedDevices.Include(od => od.Room).Include(od => od.Device).Include(od => od.Home)
            .ThenInclude(h => h.Members)
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

    public void Rename(OwnedDevice ownedDevice, string newName)
    {
        ownedDevice.Name = newName;
        _context.OwnedDevices.Update(ownedDevice);
        _context.SaveChanges();
    }

    public void UpdateLampState(Guid hardwareId, bool state)
    {
        var lamp = (LampOwnedDevice)GetByHardwareId(hardwareId);
        lamp.State = state;
        Update(lamp);
    }

    public void UpdateSensorState(Guid hardwareId, bool state)
    {
        var sensor = (SensorOwnedDevice)GetByHardwareId(hardwareId);
        sensor.IsOpen = state;
        Update(sensor);
    }

    public bool GetLampState(Guid hardwareId)
    {
        return ((LampOwnedDevice)GetByHardwareId(hardwareId)).State;
    }

    public bool GetSensorState(Guid hardwareId)
    {
        return ((SensorOwnedDevice)GetByHardwareId(hardwareId)).IsOpen;
    }

    public OwnedDevice GetOwnedDeviceById(Guid ownedDeviceId)
    {
        return _context.OwnedDevices.First(d => d.HardwareId == ownedDeviceId);
    }

    public void UpdateOwnedDevice(OwnedDevice ownedDevice)
    {
        _context.OwnedDevices.Update(ownedDevice);
        _context.SaveChanges();
    }
}
