using BusinessLogic.Roles.Entities;

namespace HomeConnect.DataAccess.Repositories;

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
        EnsureRoleIsNotNull(role);
        return role!;
    }

    private static void EnsureRoleIsNotNull(Role? role)
    {
        if (role == null)
        {
            throw new ArgumentException("Role does not exist");
        }
    }
}
