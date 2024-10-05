using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HomeConnect.DataAccess.Repositories;

public class HomeRepository : IHomeRepository
{
    private readonly Context _context = null!;

    public HomeRepository(Context context)
    {
        _context = context;
    }

    public void Add(Home home)
    {
        EnsureHomeDoesNotExist(home);
        _context.Homes.Add(home);
        _context.SaveChanges();
    }

    private void EnsureHomeDoesNotExist(Home home)
    {
        if (_context.Homes.Any(h => h.Address == home.Address))
        {
            throw new ArgumentException("Home already exists");
        }
    }

    public Home Get(Guid homeId)
    {
        Home? home = _context.Homes.FirstOrDefault(h => h.Id == homeId);
        if (home == null)
        {
            throw new ArgumentException("Home does not exist");
        }

        return home;
    }

    public Member GetMemberById(Guid memberId)
    {
        Home? home = _context.Homes.Include(h => h.Members).FirstOrDefault(h => h.Members.Any(m => m.Id == memberId));
        Member? member = home?.Members.FirstOrDefault(m => m.Id == memberId);
        return member;
    }

    public void UpdateMember(Member member)
    {
        throw new NotImplementedException();
    }

    public Home? GetByAddress(string argsAddress)
    {
        throw new NotImplementedException();
    }
}
