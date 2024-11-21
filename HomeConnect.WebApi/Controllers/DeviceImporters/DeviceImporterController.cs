using BusinessLogic.Devices.Services;
using BusinessLogic.Roles.Entities;
using HomeConnect.WebApi.Controllers.DeviceImporters.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.DeviceImporters;

[Route("device_importers")]
[ApiController]
[AuthenticationFilter]
public sealed class DeviceImporterController : ControllerBase
{
    private readonly IImporterService _importerService;

    public DeviceImporterController(IImporterService importerService)
    {
        _importerService = importerService;
    }

    [HttpGet]
    [AuthorizationFilter(SystemPermission.GetDeviceImporters)]
    public GetImportersResponse GetImporters()
    {
        var importers = _importerService.GetImporters();
        return new GetImportersResponse { Importers = importers };
    }
}
