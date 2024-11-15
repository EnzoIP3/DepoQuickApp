using BusinessLogic.Devices.Services;
using BusinessLogic.Roles.Entities;
using HomeConnect.WebApi.Controllers.DeviceImportFiles.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.DeviceImportFiles;

[Route("device_import_files")]
[ApiController]
[AuthenticationFilter]
public class DeviceImportFilesController
{
    private readonly IImporterService _importerService;

    public DeviceImportFilesController(IImporterService importerService)
    {
        _importerService = importerService;
    }

    [HttpGet]
    [AuthorizationFilter(SystemPermission.GetDeviceImportFiles)]
    public GetImportFilesResponse GetImportFiles()
    {
        var files = _importerService.GetImportFiles();
        return new GetImportFilesResponse { ImportFiles = files };
    }
}
