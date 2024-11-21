using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.Users.Entities;

namespace HomeConnect.WebApi.Controllers.Devices.Models;

public struct ImportDevicesRequest
{
    public string ImporterName { get; set; }
    public Dictionary<string, string> Parameters { get; set; }

    public ImportDevicesArgs ToImportDevicesArgs(User? user)
    {
        return new ImportDevicesArgs { ImporterName = ImporterName, User = user!, Parameters = Parameters };
    }
}
