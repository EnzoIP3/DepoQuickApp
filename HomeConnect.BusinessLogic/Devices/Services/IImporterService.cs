using BusinessLogic.BusinessOwners.Models;

namespace BusinessLogic.Devices.Services;

public interface IImporterService
{
    public List<string> GetImporters();
    List<string> ImportDevices(ImportDevicesArgs args);
    List<string> GetImportFiles();
}
