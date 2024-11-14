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
    public DbSet<Camera> Cameras { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<Business> Businesses { get; set; } = null!;
    public DbSet<Home> Homes { get; set; } = null!;
    public DbSet<OwnedDevice> OwnedDevices { get; set; } = null!;
    public DbSet<Token> Tokens { get; set; } = null!;
    public DbSet<Member> Members { get; set; } = null!;
    public DbSet<HomePermission> HomePermissions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SeedRoles(modelBuilder);
        SeedPermissions(modelBuilder);
        ConfigureRolePermissions(modelBuilder);
        ConfigureUserRole(modelBuilder);
        ConfigureMemberRelations(modelBuilder);
        ConfigureOwnedDevices(modelBuilder);
        SeedAdminUser(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private static void ConfigureOwnedDevices(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OwnedDevice>()
            .HasDiscriminator<string>("DeviceType")
            .HasValue<OwnedDevice>("OwnedDevice")
            .HasValue<LampOwnedDevice>("LampOwnedDevice")
            .HasValue<SensorOwnedDevice>("SensorOwnedDevice");
    }

    private void SeedRoles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role { Name = Role.Admin },
            new Role { Name = Role.HomeOwner },
            new Role { Name = Role.BusinessOwner });
    }

    private void SeedPermissions(ModelBuilder modelBuilder)
    {
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
            new SystemPermission { Value = SystemPermission.GetNotifications },
            new SystemPermission { Value = SystemPermission.CreateBusiness },
            new SystemPermission { Value = SystemPermission.CreateCamera },
            new SystemPermission { Value = SystemPermission.CreateSensor },
            new SystemPermission { Value = SystemPermission.UpdateMember },
            new SystemPermission { Value = SystemPermission.CreateMotionSensor },
            new SystemPermission { Value = SystemPermission.CreateLamp },
            new SystemPermission { Value = SystemPermission.UpdateBusinessValidator },
            new SystemPermission { Value = SystemPermission.GetDeviceValidators },
            new SystemPermission { Value = SystemPermission.ImportDevices },
            new SystemPermission { Value = SystemPermission.GetDeviceImportFiles },
            new SystemPermission { Value = SystemPermission.GetBusinesses },
            new SystemPermission { Value = SystemPermission.GetBusinessDevices },
            new SystemPermission { Value = SystemPermission.GetDeviceImporters });
    }

    private void ConfigureRolePermissions(ModelBuilder modelBuilder)
    {
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
                new { RolesName = Role.HomeOwner, PermissionsValue = SystemPermission.UpdateMember },
                new { RolesName = Role.HomeOwner, PermissionsValue = SystemPermission.GetNotifications },
                new { RolesName = Role.BusinessOwner, PermissionsValue = SystemPermission.CreateBusiness },
                new { RolesName = Role.BusinessOwner, PermissionsValue = SystemPermission.CreateCamera },
                new { RolesName = Role.BusinessOwner, PermissionsValue = SystemPermission.CreateSensor },
                new { RolesName = Role.BusinessOwner, PermissionsValue = SystemPermission.CreateMotionSensor },
                new { RolesName = Role.BusinessOwner, PermissionsValue = SystemPermission.CreateLamp },
                new { RolesName = Role.BusinessOwner, PermissionsValue = SystemPermission.UpdateBusinessValidator },
                new { RolesName = Role.BusinessOwner, PermissionsValue = SystemPermission.GetDeviceValidators },
                new { RolesName = Role.BusinessOwner, PermissionsValue = SystemPermission.ImportDevices },
                new { RolesName = Role.BusinessOwner, PermissionsValue = SystemPermission.GetDeviceImportFiles },
                new { RolesName = Role.BusinessOwner, PermissionsValue = SystemPermission.GetBusinesses },
                new { RolesName = Role.BusinessOwner, PermissionsValue = SystemPermission.GetBusinessDevices },
                new { RolesName = Role.BusinessOwner, PermissionsValue = SystemPermission.GetDeviceImporters }));
    }

    private void ConfigureUserRole(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany("Users")
            .UsingEntity(j => j.ToTable("UserRole").HasData(
                new { UsersId = Guid.Parse("f1b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b"), RolesName = Role.Admin }));
    }

    private void ConfigureMemberRelations(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Member>()
            .HasOne(m => m.User)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Member>()
            .HasOne(m => m.Home)
            .WithMany(u => u.Members)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Member>()
            .HasMany(m => m.HomePermissions)
            .WithMany()
            .UsingEntity(j => j.ToTable("MemberHomePermissions"));
    }

    private void SeedAdminUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(new
        {
            Id = Guid.Parse("f1b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b"),
            Name = "Administrator",
            Surname = "Account",
            Email = "admin@admin.com",
            Password = "Admin123@",
            CreatedAt = new DateOnly(2024, 1, 1)
        });
    }
}
