using BusinessLogic.Devices.Services;
using HomeConnect.WebApi.Controllers.Devices.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.DeviceImporters;

[Route("device_importers")]
[ApiController]
[AuthenticationFilter]
public class DeviceImporter : ControllerBase
{
    private readonly IImporterService _importerService;

    public DeviceImporter(IImporterService importerService)
    {
        _importerService = importerService;
    }

    [HttpGet]
    [Route("importers")]
    public GetImportersResponse GetImporters()
    {
        var importers = _importerService.GetImporters();
        return new GetImportersResponse { Importers = importers };
    }
}
