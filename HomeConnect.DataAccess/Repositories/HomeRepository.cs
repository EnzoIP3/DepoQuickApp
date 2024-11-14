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
        EnsureHomeDoesNotExist(home);
        _context.Homes.Add(home);
        _context.SaveChanges();
    }

    public Home Get(Guid homeId)
    {
        Home? home = _context.Homes.Include(h => h.Members).ThenInclude(h => h.User).Include(h => h.Members)
            .ThenInclude(h => h.HomePermissions).Include(h => h.Owner)
            .FirstOrDefault(h => h.Id == homeId);
        if (home == null)
        {
            throw new ArgumentException("Home does not exist");
        }

        return home;
    }

    public Member GetMemberById(Guid memberId)
    {
        Home home = _context.Homes.Include(home => home.Members).ThenInclude(member => member.User)
            .Include(home => home.Owner)
            .First(m => m.Members.Any(h => h.Id == memberId));
        Member member = home.Members.First(m => m.Id == memberId);
        return member;
    }

    public void UpdateMember(Member member)
    {
        Home home = _context.Homes.Include(home => home.Members)
            .First(m => m.Members.Any(h => h.Id == member.Id));
        Member memberToUpdate = home.Members.First(m => m.Id == member.Id);
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

    public bool ExistsMember(Guid memberId)
    {
        return _context.Homes.Include(h => h.Members).Any(h => h.Members.Any(m => m.Id == memberId));
    }

    public void Rename(Home home, string newName)
    {
        home.NickName = newName;
        _context.Homes.Update(home);
        _context.SaveChanges();
    }

    public void AddRoom(Room room)
    {
        if (ExistsRoom(room.Id))
        {
            throw new ArgumentException("Room already exists");
        }

        _context.Rooms.Add(room);
        _context.SaveChanges();
    }

    public Room GetRoomById(Guid parse)
    {
        throw new NotImplementedException();
    }

    public bool ExistsRoom(Guid roomId)
    {
        return _context.Rooms.Any(r => r.Id == roomId);
    }

    public void UpdateRoom(Room room)
    {
        throw new NotImplementedException();
    }

    private void EnsureHomeDoesNotExist(Home home)
    {
        if (_context.Homes.Any(h => h.Address == home.Address))
        {
            throw new ArgumentException("Home already exists");
        }
    }
}
