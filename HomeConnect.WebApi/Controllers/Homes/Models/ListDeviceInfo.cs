using BusinessLogic.Devices.Entities;

namespace HomeConnect.WebApi.Controllers.Homes.Models;

public record ListDeviceInfo
{
    public string HardwareId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string BusinessName { get; set; } = null!;
    public string Type { get; set; } = null!;
    public int ModelNumber { get; set; }
    public string Photo { get; set; } = null!;

    public static ListDeviceInfo FromOwnedDevice(OwnedDevice ownedDevice)
    {
        return new ListDeviceInfo
        {
            HardwareId = ownedDevice.HardwareId.ToString(),
            Name = ownedDevice.Device.Name,
            BusinessName = ownedDevice.Device.Business.Name,
            Type = ownedDevice.Device.Type.ToString(),
            ModelNumber = ownedDevice.Device.ModelNumber,
            Photo = ownedDevice.Device.MainPhoto
        };
    }
}
