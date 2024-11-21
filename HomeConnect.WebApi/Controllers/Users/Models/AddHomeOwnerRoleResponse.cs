using BusinessLogic.Users.Entities;

namespace HomeConnect.WebApi.Controllers.Users.Models;

public sealed record AddHomeOwnerRoleResponse
{
    public string Id { get; init; } = string.Empty;
    public Dictionary<string, List<string>> Roles { get; set; } = null!;

    public static AddHomeOwnerRoleResponse FromUser(User user)
    {
        return new AddHomeOwnerRoleResponse
        {
            Id = user.Id.ToString(),
            Roles = user.GetRolesAndPermissions()
                .ToDictionary(x => x.Key.Name, x => x.Value.Select(y => y.Value).ToList())
        };
    }
}
