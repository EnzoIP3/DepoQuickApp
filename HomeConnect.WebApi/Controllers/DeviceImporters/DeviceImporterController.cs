using BusinessLogic.Devices.Services;
using HomeConnect.WebApi.Controllers.DeviceImporters.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.DeviceImporters;

[Route("device_importers")]
[ApiController]
[AuthenticationFilter]
public class DeviceImporterController : ControllerBase
{
    private readonly IImporterService _importerService;

    public DeviceImporterController(IImporterService importerService)
    {
        _importerService = importerService;
    }

    [HttpGet]
    public GetImportersResponse GetImporters()
    {
        var importers = _importerService.GetImporters();
        return new GetImportersResponse { Importers = importers };
    }
}
