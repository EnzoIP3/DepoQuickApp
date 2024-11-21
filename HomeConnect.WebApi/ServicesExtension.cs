using BusinessLogic.Admins.Services;
using BusinessLogic.Auth.Repositories;
using BusinessLogic.Auth.Services;
using BusinessLogic.BusinessOwners.Repositories;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Repositories;
using BusinessLogic.Devices.Services;
using BusinessLogic.Helpers;
using BusinessLogic.HomeOwners.Repositories;
using BusinessLogic.HomeOwners.Services;
using BusinessLogic.Notifications.Repositories;
using BusinessLogic.Notifications.Services;
using BusinessLogic.Roles.Repositories;
using BusinessLogic.Users.Repositories;
using BusinessLogic.Users.Services;
using DeviceImporter;
using HomeConnect.DataAccess;
using HomeConnect.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using ModeloValidador.Abstracciones;

namespace HomeConnect.WebApi;

public static class ServicesExtension
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Missing DefaultConnection connection string");
        }

        services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IHomeRepository, HomeRepository>();
        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<IBusinessRepository, BusinessRepository>();
        services.AddScoped<IOwnedDeviceRepository, OwnedDeviceRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IHomeOwnerService, HomeOwnerService>();
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IBusinessOwnerService, BusinessOwnerService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IValidatorService, ValidatorService>();
        services.AddScoped<IImporterService, ImporterService>();
        services.AddScoped<IAssemblyInterfaceLoader<IDeviceImporter>, AssemblyInterfaceLoader<IDeviceImporter>>();
        services.AddScoped<IAssemblyInterfaceLoader<IModeloValidador>, AssemblyInterfaceLoader<IModeloValidador>>();
        return services;
    }
}
