using BusinessLogic.HomeOwners.Entities;

namespace BusinessLogic.HomeOwners.Repositories;

public interface IMemberRepository
{
    void Add(Member member);
    List<Member> GetMembersByUserId(Guid userId);
}
