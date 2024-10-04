using BusinessLogic.Devices.Repositories;

namespace BusinessLogic.Devices.Services;

public class DeviceService(IOwnedDeviceRepository ownedDeviceRepository) : IDeviceService
{
    public bool Toggle(string hardwareId)
    {
        EnsureHardwareIdIsValid(hardwareId);
        var connectionState = ownedDeviceRepository.ToggleConnection(hardwareId);
        return connectionState;
    }

    private void EnsureHardwareIdIsValid(string hardwareId)
    {
        if (string.IsNullOrWhiteSpace(hardwareId) || !Guid.TryParse(hardwareId, out _))
        {
            throw new ArgumentException("Hardware ID is invalid");
        }
    }
}
