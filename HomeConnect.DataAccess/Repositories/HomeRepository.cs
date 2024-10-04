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
        _context.Homes.Add(home);
        _context.SaveChanges();
    }

    public Home Get(Guid homeId)
    {
        throw new NotImplementedException();
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
