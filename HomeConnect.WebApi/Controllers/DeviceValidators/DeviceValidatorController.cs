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
public class DeviceValidatorController : ControllerBase
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
        var validators = _validatorService.GetValidators();
        return CreateGetValidatorsResponse(validators);
    }

    private static GetValidatorsResponse CreateGetValidatorsResponse(List<ValidatorInfo> validators)
    {
        return new GetValidatorsResponse { Validators = validators.Select(v => v.Name).ToList() };
    }
}
