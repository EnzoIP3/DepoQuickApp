using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Repositories;

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
        return home;
    }

    public Member GetMemberById(Guid memberId)
    {
        throw new NotImplementedException();
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
