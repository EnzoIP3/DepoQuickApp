namespace HomeConnect.WebApi.Controllers.Users.Models;

public record AddHomeOwnerRoleResponse
{
    public string Id { get; init; } = string.Empty;
    public Dictionary<string, List<string>> Roles { get; set; } = null!;
}
