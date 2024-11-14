namespace HomeConnect.WebApi.Controllers.Homes.Models;

public record GetHomePermissionsResponse
{
    public string HomeId { get; init; } = null!;
    public List<string> HomePermissions { get; init; } = null!;
}
