using BusinessLogic.HomeOwners.Entities;

namespace HomeConnect.WebApi.Controllers.Homes.Models;

public class GetRoomsResponse
{
    public List<ListRoomInfo> Rooms { get; set; } = [];

    public static GetRoomsResponse FromRooms(IEnumerable<Room> rooms)
    {
        var roomInfos = rooms.Select(ListRoomInfo.FromRoom).ToList();
        return new GetRoomsResponse { Rooms = roomInfos };
    }
}
