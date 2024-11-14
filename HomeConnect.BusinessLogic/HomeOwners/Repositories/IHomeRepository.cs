using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Entities;

namespace BusinessLogic.HomeOwners.Repositories;

public interface IHomeRepository
{
    void Add(Home home);
    Home Get(Guid homeId);
    Member GetMemberById(Guid memberId);
    void UpdateMember(Member member);
    Home? GetByAddress(string argsAddress);
    bool Exists(Guid homeId);
    bool ExistsMember(Guid memberId);
    void Rename(Home home, string newName);
    void AddRoom(Room room);
    Room GetRoomById(Guid roomId);
    bool ExistsRoom(Guid roomId);
    void UpdateRoom(Room room);
    void Update(Home home);
}
