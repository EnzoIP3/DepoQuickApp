using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Devices.Services;
using BusinessLogic.Notifications.Services;
using BusinessLogic.Roles.Entities;
using HomeConnect.WebApi.Controllers.MotionSensors.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.MotionSensors;

public class MotionSensorController : ControllerBase
{
    [HttpPost]
    [AuthenticationFilter]
    [AuthorizationFilter(SystemPermission.CreateMotionSensor)]
    public CreateMotionSensorResponse CreateMotionSensor([FromBody] CreateMotionSensorRequest request)
    {
        throw new NotImplementedException();
    }
}
