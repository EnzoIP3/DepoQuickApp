namespace BusinessLogic.HomeOwners.Models;

public record CreateHomeArgs
{
    public string HomeOwnerId { get; set; } = null!;
    public string Address { get; set; } = null!;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public int? MaxMembers { get; set; }
}
