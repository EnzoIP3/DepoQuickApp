namespace HomeConnect.WebApi.Controllers.User.Models;

public record ListUserInfo
{
    public string Id { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Surname { get; init; } = null!;
    public string Role { get; init; } = null!;
    public string CreatedAt { get; init; } = null!;
}
