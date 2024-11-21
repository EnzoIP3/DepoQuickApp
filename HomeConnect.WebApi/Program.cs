using System.Diagnostics.CodeAnalysis;
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
using HomeConnect.WebApi.Filters;
using Microsoft.EntityFrameworkCore;
using ModeloValidador.Abstracciones;

void AddDbContext(string? s, IServiceCollection services1)
{
    if (string.IsNullOrEmpty(s))
    {
        throw new InvalidOperationException("Missing DefaultConnection connection string");
    }

    services1.AddDbContext<Context>(options => options.UseSqlServer(s));
}

void AddScopedServices(IServiceCollection serviceCollection)
{
    serviceCollection.AddScoped<ITokenRepository, TokenRepository>();
    serviceCollection.AddScoped<IRoleRepository, RoleRepository>();
    serviceCollection.AddScoped<IUserRepository, UserRepository>();
    serviceCollection.AddScoped<IHomeRepository, HomeRepository>();
    serviceCollection.AddScoped<IDeviceRepository, DeviceRepository>();
    serviceCollection.AddScoped<IBusinessRepository, BusinessRepository>();
    serviceCollection.AddScoped<IOwnedDeviceRepository, OwnedDeviceRepository>();
    serviceCollection.AddScoped<INotificationRepository, NotificationRepository>();
    serviceCollection.AddScoped<IRoomRepository, RoomRepository>();
    serviceCollection.AddScoped<IMemberRepository, MemberRepository>();
    serviceCollection.AddScoped<IAuthService, AuthService>();
    serviceCollection.AddScoped<IUserService, UserService>();
    serviceCollection.AddScoped<IHomeOwnerService, HomeOwnerService>();
    serviceCollection.AddScoped<IDeviceService, DeviceService>();
    serviceCollection.AddScoped<IAdminService, AdminService>();
    serviceCollection.AddScoped<IBusinessOwnerService, BusinessOwnerService>();
    serviceCollection.AddScoped<INotificationService, NotificationService>();
    serviceCollection.AddScoped<IValidatorService, ValidatorService>();
    serviceCollection.AddScoped<IImporterService, ImporterService>();
    serviceCollection.AddScoped<IAssemblyInterfaceLoader<IDeviceImporter>, AssemblyInterfaceLoader<IDeviceImporter>>();
    serviceCollection.AddScoped<IAssemblyInterfaceLoader<IModeloValidador>, AssemblyInterfaceLoader<IModeloValidador>>();
}

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCors(options =>
    {
        options.AddDefaultPolicy(
            b => b.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
    })
    .AddControllers(
        options =>
        {
            options.Filters.Add<ExceptionFilter>();
        })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressMapClientErrors = true;
    });

var services = builder.Services;
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DefaultConnection");

AddDbContext(connectionString, services);

AddScopedServices(services);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors();

app.Run();

namespace HomeConnect.WebApi
{
    [ExcludeFromCodeCoverage]
    public partial class Program
    {
    }
}
