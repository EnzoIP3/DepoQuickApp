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
            new Role { Name = Role.Admin },
            new Role { Name = Role.HomeOwner },
            new Role { Name = Role.BusinessOwner });

        modelBuilder.Entity<SystemPermission>().HasData(
            new SystemPermission { Value = SystemPermission.CreateAdministrator },
            new SystemPermission { Value = SystemPermission.DeleteAdministrator },
            new SystemPermission { Value = SystemPermission.CreateBusinessOwner },
            new SystemPermission { Value = SystemPermission.GetAllUsers },
            new SystemPermission { Value = SystemPermission.GetAllBusinesses },
            new SystemPermission { Value = SystemPermission.CreateHome },
            new SystemPermission { Value = SystemPermission.AddMember },
            new SystemPermission { Value = SystemPermission.AddDevice },
            new SystemPermission { Value = SystemPermission.GetDevices },
            new SystemPermission { Value = SystemPermission.GetMembers },
            new SystemPermission { Value = SystemPermission.CreateBusiness },
            new SystemPermission { Value = SystemPermission.CreateCamera },
            new SystemPermission { Value = SystemPermission.CreateSensor });

        modelBuilder.Entity<Role>()
            .HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity(j => j.HasData(
                new { RolesName = Role.Admin, PermissionsValue = SystemPermission.CreateAdministrator },
                new { RolesName = Role.Admin, PermissionsValue = SystemPermission.DeleteAdministrator },
                new { RolesName = Role.Admin, PermissionsValue = SystemPermission.CreateBusinessOwner },
                new { RolesName = Role.Admin, PermissionsValue = SystemPermission.GetAllUsers },
                new { RolesName = Role.Admin, PermissionsValue = SystemPermission.GetAllBusinesses },
                new { RolesName = Role.HomeOwner, PermissionsValue = SystemPermission.CreateHome },
                new { RolesName = Role.HomeOwner, PermissionsValue = SystemPermission.AddMember },
                new { RolesName = Role.HomeOwner, PermissionsValue = SystemPermission.AddDevice },
                new { RolesName = Role.HomeOwner, PermissionsValue = SystemPermission.GetDevices },
                new { RolesName = Role.HomeOwner, PermissionsValue = SystemPermission.GetMembers },
                new { RolesName = Role.BusinessOwner, PermissionsValue = SystemPermission.CreateBusiness },
                new { RolesName = Role.BusinessOwner, PermissionsValue = SystemPermission.CreateCamera },
                new { RolesName = Role.BusinessOwner, PermissionsValue = SystemPermission.CreateSensor }));

        base.OnModelCreating(modelBuilder);
    }
}
