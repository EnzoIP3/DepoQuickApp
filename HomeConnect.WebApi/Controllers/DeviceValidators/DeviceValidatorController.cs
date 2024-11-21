using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using BusinessLogic.Roles.Entities;
using HomeConnect.WebApi.Controllers.DeviceValidators.Models;
using HomeConnect.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HomeConnect.WebApi.Controllers.DeviceValidators;

[Route("device_validators")]
[ApiController]
[AuthenticationFilter]
public sealed class DeviceValidatorController : ControllerBase
{
    private readonly IValidatorService _validatorService;

    public DeviceValidatorController(IValidatorService validatorService)
    {
        _validatorService = validatorService;
    }

    [HttpGet]
    [AuthorizationFilter(SystemPermission.GetDeviceValidators)]
    public GetValidatorsResponse GetValidators()
    {
        List<ValidatorInfo> validators = _validatorService.GetValidators();
        return GetValidatorsResponse.FromValidators(validators);
    }
}
