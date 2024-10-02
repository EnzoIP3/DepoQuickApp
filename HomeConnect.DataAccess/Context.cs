using BusinessLogic;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.Notifications.Entities;
using BusinessLogic.Roles.Entities;
using BusinessLogic.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeConnect.DataAccess;

public class Context(DbContextOptions<Context> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<SystemPermission> Permissions { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Device> Devices { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<Business> Businesses { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role("Admin", []),
            new Role("Business Owner", []),
            new Role("Home Owner", []));

        base.OnModelCreating(modelBuilder);
    }
}
