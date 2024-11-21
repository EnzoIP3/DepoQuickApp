using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.Users.Entities;

namespace HomeConnect.WebApi.Controllers.Businesses.Models;

public sealed record CreateBusinessRequest
{
    public string? Name { get; set; }
    public string? Logo { get; set; }
    public string? Rut { get; set; }
    public string? Validator { get; set; }

    public CreateBusinessArgs ToCreateBusinessArgs(User? user)
    {
        return new CreateBusinessArgs
        {
            Name = Name ?? string.Empty,
            Logo = Logo ?? string.Empty,
            OwnerId = user?.Id.ToString() ?? string.Empty,
            Rut = Rut ?? string.Empty,
            Validator = Validator ?? string.Empty
        };
    }
}
