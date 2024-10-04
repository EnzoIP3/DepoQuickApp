using BusinessLogic.Devices.Repositories;

namespace BusinessLogic.Devices.Services;

public class DeviceService() : IDeviceService
{
    public bool Toogle(string hardwareId)
    {
        EnsureHardwareIdIsValid(hardwareId);
        return false;
    }

    private void EnsureHardwareIdIsValid(string hardwareId)
    {
        if (string.IsNullOrWhiteSpace(hardwareId) || !Guid.TryParse(hardwareId, out _))
        {
            throw new ArgumentException("Hardware ID is invalid");
        }
    }
}
