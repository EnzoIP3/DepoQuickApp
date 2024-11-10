using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.BusinessOwners.Services;
using HomeConnect.WebApi.Controllers.Devices.Models;
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
