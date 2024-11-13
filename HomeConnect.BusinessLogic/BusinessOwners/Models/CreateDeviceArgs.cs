using BusinessLogic.Users.Entities;

namespace BusinessLogic.BusinessOwners.Models;

public record CreateDeviceArgs
{
    public string Name { get; set; } = null!;
    public User Owner { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? ModelNumber { get; set; }
    public string MainPhoto { get; set; } = null!;
    public List<string>? SecondaryPhotos { get; set; }
    public string Type { get; set; } = null!;
}
