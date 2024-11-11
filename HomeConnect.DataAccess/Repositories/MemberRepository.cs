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
}
