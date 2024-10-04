using BusinessLogic.HomeOwners.Entities;

namespace BusinessLogic.HomeOwners.Repositories;

public interface IHomeRepository
{
    void Add(Home home);
    Home Get(Guid homeId);
    Member GetMemberById(Guid memberId);
    void UpdateMember(Member member);
    Home? GetByAddress(string argsAddress);
}
