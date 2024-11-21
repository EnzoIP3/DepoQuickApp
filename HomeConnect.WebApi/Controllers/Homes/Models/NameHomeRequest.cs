using BusinessLogic.HomeOwners.Models;
using BusinessLogic.Users.Entities;

namespace HomeConnect.WebApi.Controllers.Homes.Models;

public sealed class NameHomeRequest
{
    public string? NewName { get; set; }

    public NameHomeArgs ToArgs(string homesId, User owner)
    {
        return new NameHomeArgs { HomeId = Guid.Parse(homesId), NewName = NewName ?? string.Empty, OwnerId = owner.Id };
    }
}
