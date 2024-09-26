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
        var role = _context.Roles.FirstOrDefault(r => r.Name == name);
        if (role == null)
        {
            throw new ArgumentException("Role does not exist");
        }

        return role;
    }
}
