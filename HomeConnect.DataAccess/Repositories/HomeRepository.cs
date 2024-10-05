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
        Home? home = _context.Homes.Include(home => home.Members).FirstOrDefault(m => m.Members.Any(h => h.Id == memberId));
        Member? member = null;
        if (home != null)
        {
            member = home?.Members.FirstOrDefault(m => m.Id == memberId);
        }

        if (home == null || member == null)
        {
            throw new ArgumentException("Member does not exist");
        }

        return member;
    }

    public void UpdateMember(Member member)
    {
        Member? memberToUpdate = null;
        Home? home = _context.Homes.Include(home => home.Members).FirstOrDefault(m => m.Members.Any(h => h.Id == member.Id));
        if (home != null)
        {
            memberToUpdate = home.Members.FirstOrDefault(m => m.Id == member.Id);
        }

        if (home == null || memberToUpdate == null)
        {
            throw new ArgumentException("Member does not exist");
        }

        memberToUpdate.HomePermissions = member.HomePermissions;
        _context.SaveChanges();
    }

    public Home? GetByAddress(string argsAddress)
    {
        return _context.Homes.FirstOrDefault(h => h.Address == argsAddress);
    }

    public bool Exists(Guid homeId)
    {
        return _context.Homes.Any(h => h.Id == homeId);
    }
}
