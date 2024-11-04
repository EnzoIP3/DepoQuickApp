using BusinessLogic.BusinessOwners.Models;

namespace BusinessLogic.BusinessOwners.Services;

public interface IImporterService
{
    public List<string> GetImporters();
    List<string> ImportDevices(ImportDevicesArgs args);
    List<string> GetImportFiles();
}
