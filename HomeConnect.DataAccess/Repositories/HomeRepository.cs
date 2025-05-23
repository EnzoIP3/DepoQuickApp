using BusinessLogic.HomeOwners.Entities;
using BusinessLogic.HomeOwners.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HomeConnect.DataAccess.Repositories;

public class HomeRepository : IHomeRepository
{
    private readonly Context _context;

    public HomeRepository(Context context)
    {
        _context = context;
    }

    public void Add(Home home)
    {
        _context.Homes.Add(home);
        _context.SaveChanges();
    }

    public Home Get(Guid homeId)
    {
        return _context.Homes.Include(h => h.Members).ThenInclude(h => h.User).Include(h => h.Members)
            .ThenInclude(h => h.HomePermissions).Include(h => h.Owner)
            .First(h => h.Id == homeId);
    }

    public Home? GetByAddress(string argsAddress)
    {
        return _context.Homes.FirstOrDefault(h => h.Address == argsAddress);
    }

    public bool Exists(Guid homeId)
    {
        return _context.Homes.Any(h => h.Id == homeId);
    }

    public void Rename(Home home, string newName)
    {
        home.NickName = newName;
        _context.Homes.Update(home);
        _context.SaveChanges();
    }

    public List<Home> GetHomesByUserId(Guid userId)
    {
        return _context.Homes.Include(h => h.Members).ThenInclude(m => m.User).Include(h => h.Owner)
            .Where(h => h.Owner.Id == userId || h.Members.Any(m => m.User.Id == userId)).ToList();
    }

    public void Update(Home home)
    {
        _context.Homes.Update(home);
        _context.SaveChanges();
    }
}
