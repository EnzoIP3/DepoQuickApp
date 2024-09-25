using BusinessLogic;

namespace HomeConnect.DataAccess;

public class BusinessRepository
{
    private readonly Context _context;

    public BusinessRepository(Context context)
    {
        _context = context;
    }

    public List<Business> GetBusinesses(int page, int pageSize)
    {
        return _context.Businesses.ToList();
    }
}
