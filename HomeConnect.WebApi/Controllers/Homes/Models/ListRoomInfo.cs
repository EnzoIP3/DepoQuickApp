using BusinessLogic.HomeOwners.Entities;

namespace HomeConnect.WebApi.Controllers.Homes.Models;

public record ListRoomInfo
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string HomeId { get; set; } = null!;
    public List<string> OwnedDevicesId { get; set; } = null!;

    public static ListRoomInfo FromRoom(Room room)
    {
        var dto = room.ToRoomDto();
        return new ListRoomInfo
        {
            Id = dto.Id,
            Name = dto.Name,
            HomeId = dto.HomeId,
            OwnedDevicesId = dto.OwnedDevicesId
        };
    }
}
