using BusinessLogic.HomeOwners.Entities;

namespace HomeConnect.DataAccess.Repositories;

public class RoomRepository
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
        throw new NotImplementedException();
    }
}
