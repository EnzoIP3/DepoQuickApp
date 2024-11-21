using System.Diagnostics.CodeAnalysis;
using HomeConnect.WebApi;
using HomeConnect.WebApi.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDatabase(builder.Configuration)
    .AddServices()
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

WebApplication app = builder.Build();

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
