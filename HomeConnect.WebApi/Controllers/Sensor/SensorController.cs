using BusinessLogic.Notifications.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Sensor;

[ApiController]
[Route("sensors")]
public class SensorController() : ControllerBase
{
    [HttpPost("{hardwareId}/open")]
    public NotifyResponse Notify([FromRoute] string hardwareId)
    {
        throw new NotImplementedException();
    }
}
