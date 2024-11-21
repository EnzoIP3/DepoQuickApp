using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.Devices.Models;

namespace BusinessLogic.Devices.Services;

public interface IImporterService
{
    public List<ImporterData> GetImporters();
    List<string> ImportDevices(ImportDevicesArgs args);
    List<string> GetImportFiles();
}
