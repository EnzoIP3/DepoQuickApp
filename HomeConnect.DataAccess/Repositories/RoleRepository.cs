using BusinessLogic.Roles.Entities;
using BusinessLogic.Roles.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HomeConnect.DataAccess.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly Context _context;

    public RoleRepository(Context context)
    {
        _context = context;
    }

    public Role Get(string name)
    {
        Role? role = _context.Roles.Include(r => r.Permissions).FirstOrDefault(r => r.Name == name);
        EnsureRoleIsNotNull(role);
        return role!;
    }

    public bool Exists(string name)
    {
        return _context.Roles.Any(r => r.Name == name);
    }

    private static void EnsureRoleIsNotNull(Role? role)
    {
        if (role == null)
        {
            throw new ArgumentException("Role does not exist");
        }
    }
}
