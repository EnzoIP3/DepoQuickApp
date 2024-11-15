using BusinessLogic.HomeOwners.Entities;

namespace BusinessLogic.HomeOwners.Repositories;

public interface IRoomRepository
{
    void Add(Room room);
    Room Get(Guid roomId);
    bool Exists(Guid roomId);
    void Update(Room updatedRoom);
}
