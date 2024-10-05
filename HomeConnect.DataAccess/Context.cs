using BusinessLogic.Auth.Entities;
using BusinessLogic.BusinessOwners.Entities;
using BusinessLogic.Devices.Entities;
using BusinessLogic.HomeOwners.Entities;
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
    public DbSet<Home> Homes { get; set; } = null!;
    public DbSet<OwnedDevice> OwnedDevices { get; set; } = null!;
    public DbSet<Token> Tokens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role(Role.Admin, []),
            new Role(Role.HomeOwner, []),
            new Role(Role.BusinessOwner, []));

        modelBuilder.Entity<SystemPermission>().HasData(
            new SystemPermission(SystemPermission.CreateAdministrator, Role.Admin),
            new SystemPermission(SystemPermission.DeleteAdministrator, Role.Admin),
            new SystemPermission(SystemPermission.CreateBusinessOwner, Role.Admin),
            new SystemPermission(SystemPermission.GetAllUsers, Role.Admin),
            new SystemPermission(SystemPermission.GetAllBusinesses, Role.Admin),
            new SystemPermission(SystemPermission.CreateHome, Role.HomeOwner),
            new SystemPermission(SystemPermission.AddMember, Role.HomeOwner),
            new SystemPermission(SystemPermission.AddDevice, Role.HomeOwner),
            new SystemPermission(SystemPermission.GetDevices, Role.HomeOwner),
            new SystemPermission(SystemPermission.GetMembers, Role.HomeOwner),
            new SystemPermission(SystemPermission.CreateBusiness, Role.BusinessOwner),
            new SystemPermission(SystemPermission.CreateCamera, Role.BusinessOwner),
            new SystemPermission(SystemPermission.CreateSensor, Role.BusinessOwner));

        base.OnModelCreating(modelBuilder);
    }
}
