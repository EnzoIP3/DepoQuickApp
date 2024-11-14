namespace HomeConnect.WebApi.Controllers.Homes.Models;

public record OwnerInfo
{
    public string Id { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Surname { get; init; } = null!;
    public string? ProfilePicture { get; init; } = null!;
}
