using BusinessLogic.Devices.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.Camera;

[ApiController]
[Route("cameras")]
public class CameraController(IDeviceService deviceService) : BaseDeviceController(deviceService);
