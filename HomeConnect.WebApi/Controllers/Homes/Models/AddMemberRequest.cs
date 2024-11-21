using BusinessLogic.HomeOwners.Models;

namespace HomeConnect.WebApi.Controllers.Homes.Models;

public record AddMemberRequest
{
    public string? Email { get; set; } = null!;
    public List<string> Permissions { get; set; } = [];

    public AddMemberArgs ToArgs(string homeId)
    {
        return new AddMemberArgs { HomeId = homeId, UserEmail = Email ?? string.Empty, Permissions = Permissions };
    }
}
