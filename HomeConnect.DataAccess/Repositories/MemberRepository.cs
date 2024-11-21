using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HomeConnect.DataAccess.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly Context _context;

    public MemberRepository(Context context)
    {
        _context = context;
    }

    public void Add(Member member)
    {
        _context.Members.Add(member);
        _context.SaveChanges();
    }

    public Member Get(Guid memberId)
    {
        return _context.Members.Include(m => m.Home).Include(m => m.HomePermissions)
            .First(m => m.Id == memberId);
    }

    public void Update(Member member)
    {
        _context.Members.Update(member);
        _context.SaveChanges();
    }

    public bool Exists(Guid memberId)
    {
        return _context.Members.Any(m => m.Id == memberId);
    }
}
