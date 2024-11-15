using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Repositories;

namespace HomeConnect.DataAccess.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly Context _context;

    public RoomRepository(Context context)
    {
        _context = context;
    }

    public void Add(Room room)
    {
        _context.Rooms.Add(room);
        _context.SaveChanges();
    }

    public Room Get(Guid roomId)
    {
        return _context.Rooms.First(r => r.Id == roomId);
    }

    public bool Exists(Guid roomId)
    {
        return _context.Rooms.Any(r => r.Id == roomId);
    }

    public void Update(Room updatedRoom)
    {
        _context.Rooms.Update(updatedRoom);
        _context.SaveChanges();
    }
}
