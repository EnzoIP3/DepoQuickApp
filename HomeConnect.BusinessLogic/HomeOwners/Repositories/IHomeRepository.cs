using BusinessLogic.HomeOwners.Entities;

namespace BusinessLogic.HomeOwners.Repositories;

public interface IHomeRepository
{
    void Add(Home home);
    Home Get(Guid homeId);
    Home? GetByAddress(string argsAddress);
    bool Exists(Guid homeId);
    void Rename(Home home, string newName);
    void AddRoom(Room room);
    Room GetRoomById(Guid roomId);
    bool ExistsRoom(Guid roomId);
    void UpdateRoom(Room room);
    void Update(Home home);
    List<Home> GetHomesByUserId(Guid userId);
}
