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
        var dto = ownedDevice.ToOwnedDeviceDto();
        return new ListDeviceInfo
        {
            HardwareId = dto.HardwareId,
            Name = dto.Name,
            BusinessName = dto.BusinessName,
            Type = dto.Type,
            ModelNumber = dto.ModelNumber,
            Photo = dto.Photo
        };
    }
}
