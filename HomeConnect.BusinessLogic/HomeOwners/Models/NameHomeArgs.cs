namespace BusinessLogic.HomeOwners.Models;

public record NameHomeArgs
{
    public Guid OwnerId { get; set; }
    public Guid HomeId { get; set; }
    public string NewName { get; set; } = null!;
}
