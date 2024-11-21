namespace BusinessLogic.HomeOwners.Models;

public record NameDeviceArgs
{
    public Guid OwnerId { get; set; }
    public string HardwareId { get; set; } = null!;
    public string NewName { get; set; } = null!;
}
