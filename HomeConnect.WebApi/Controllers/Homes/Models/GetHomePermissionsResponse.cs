using BusinessLogic.HomeOwners.Entities;

namespace HomeConnect.WebApi.Controllers.Homes.Models;

public sealed record GetHomePermissionsResponse
{
    public string HomeId { get; init; } = null!;
    public List<string> HomePermissions { get; init; } = null!;

    public static GetHomePermissionsResponse FromHomePermissions(string homesId, List<HomePermission> permissions)
    {
        return new GetHomePermissionsResponse
        {
            HomeId = homesId, HomePermissions = permissions.Select(p => p.Value).ToList()
        };
    }
}
