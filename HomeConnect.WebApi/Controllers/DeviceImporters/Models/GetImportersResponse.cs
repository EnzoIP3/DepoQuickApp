using BusinessLogic.Devices.Models;

namespace HomeConnect.WebApi.Controllers.DeviceImporters.Models;

public sealed record GetImportersResponse
{
    public List<ImporterData> Importers { get; set; } = new List<ImporterData>();
}
