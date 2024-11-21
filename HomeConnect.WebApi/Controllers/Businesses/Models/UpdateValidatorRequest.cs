using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.Users.Entities;

namespace HomeConnect.WebApi.Controllers.Businesses.Models;

public sealed record UpdateValidatorRequest
{
    public string? Validator { get; set; }

    public UpdateValidatorArgs ToUpdateValidatorArgs(string businessRut, User? user)
    {
        return new UpdateValidatorArgs
        {
            BusinessRut = businessRut,
            Validator = Validator ?? string.Empty,
            OwnerId = user?.Id.ToString() ?? string.Empty
        };
    }
}
