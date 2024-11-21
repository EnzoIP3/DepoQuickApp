using BusinessLogic.HomeOwners.Entities;

namespace BusinessLogic.HomeOwners.Repositories;

public interface IMemberRepository
{
    void Add(Member member);
    Member Get(Guid memberId);
    void Update(Member member);
    bool Exists(Guid memberId);
}
