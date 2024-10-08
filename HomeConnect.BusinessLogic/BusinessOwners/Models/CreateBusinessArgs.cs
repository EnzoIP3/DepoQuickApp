namespace BusinessLogic.BusinessOwners.Models;

public record CreateBusinessArgs
{
    public string Name { get; init; } = null!;
    public string Logo { get; init; } = null!;
    public string Rut { get; init; } = null!;
    public string OwnerId { get; init; } = null!;
}
