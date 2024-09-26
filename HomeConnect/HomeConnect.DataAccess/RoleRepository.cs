using BusinessLogic;

namespace HomeConnect.DataAccess;

public class RoleRepository
{
    private readonly Context _context;

    public RoleRepository(Context context)
    {
        _context = context;
    }

    public Role GetRole(string name)
    {
        return _context.Roles.FirstOrDefault(r => r.Name == name);
    }
}
