using BusinessLogic.Devices.Entities;

namespace HomeConnect.WebApi.Controllers.Homes.Models;

public record ListDeviceInfo
{
    public string Id { get; set; } = null!;
    public string HardwareId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string BusinessName { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string ModelNumber { get; set; } = null!;
    public string MainPhoto { get; set; } = null!;
    public List<string> SecondaryPhotos { get; set; } = null!;
    public bool? State { get; set; }
    public bool? IsOpen { get; set; }
    public string? RoomId { get; set; } = null!;

    public static ListDeviceInfo FromOwnedDevice(OwnedDevice ownedDevice)
    {
        var dto = ownedDevice.ToOwnedDeviceDto();
        return new ListDeviceInfo
        {
            Id = dto.Id,
            HardwareId = dto.HardwareId,
            Name = dto.Name,
            BusinessName = dto.BusinessName,
            Type = dto.Type,
            ModelNumber = dto.ModelNumber,
            MainPhoto = dto.Photo,
            SecondaryPhotos = dto.SecondaryPhotos,
            State = dto.State,
            IsOpen = dto.IsOpen,
            RoomId = dto.RoomId
        };
    }
}
