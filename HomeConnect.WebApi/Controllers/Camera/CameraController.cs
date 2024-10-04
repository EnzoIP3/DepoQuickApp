using BusinessLogic.Devices.Services;
using HomeConnect.WebApi.Controllers.Sensor;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Camera;

[ApiController]
[Route("cameras")]
public class CameraController() : ControllerBase
{
    [HttpPost("{hardwareId}/toggle")]
    public ConnectionResponse Toggle([FromRoute] string hardwareId)
    {
        throw new NotImplementedException();
    }
}
