using BusinessLogic.BusinessOwners.Models;

namespace HomeConnect.WebApi.Controllers.DeviceValidators.Models;

public struct GetValidatorsResponse
{
    public List<string> Validators { get; set; }

    public static GetValidatorsResponse FromValidators(List<ValidatorInfo> validators)
    {
        return new GetValidatorsResponse { Validators = validators.Select(v => v.Name).ToList() };
    }
}
