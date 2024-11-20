using BusinessLogic.Devices.Models;

namespace HomeConnect.WebApi.Controllers.DeviceImporters.Models;

public struct GetImportersResponse
{
    public List<ImporterData> Importers { get; set; }
}
