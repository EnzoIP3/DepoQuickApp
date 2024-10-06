namespace BusinessLogic.BusinessOwners.Models;

public record CreateBusinessArgs
{
    public string Name { get; set; } = null!;
    public string Logo { get; set; } = null!;
    public string Rut { get; set; } = null!;
    public string OwnerId { get; set; } = null!;
}
