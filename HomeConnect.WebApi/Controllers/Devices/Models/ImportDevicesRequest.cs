using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.Users.Entities;

namespace HomeConnect.WebApi.Controllers.Devices.Models;

public sealed record ImportDevicesRequest
{
    public string ImporterName { get; set; } = null!;
    public Dictionary<string, string> Parameters { get; set; } = null!;

    public ImportDevicesArgs ToImportDevicesArgs(User? user)
    {
        return new ImportDevicesArgs { ImporterName = ImporterName, User = user!, Parameters = Parameters };
    }
}
