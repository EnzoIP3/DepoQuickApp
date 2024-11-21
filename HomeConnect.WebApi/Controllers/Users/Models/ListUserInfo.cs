namespace HomeConnect.WebApi.Controllers.Users.Models;

public sealed record ListUserInfo
{
    public string Id { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Surname { get; init; } = null!;
    public List<string> Roles { get; init; } = null!;
    public string CreatedAt { get; init; } = null!;
}
