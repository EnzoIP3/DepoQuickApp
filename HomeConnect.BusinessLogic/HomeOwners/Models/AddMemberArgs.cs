namespace BusinessLogic.HomeOwners.Models;

public record AddMemberArgs
{
    public string HomeId { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public List<string> Permissions { get; set; } = [];
}
