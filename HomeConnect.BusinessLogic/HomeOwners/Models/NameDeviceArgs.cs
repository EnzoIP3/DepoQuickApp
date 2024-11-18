namespace BusinessLogic.HomeOwners.Models;

public record NameDeviceArgs
{
    public Guid OwnerId { get; set; }
    public Guid HardwareId { get; set; }
    public string NewName { get; set; } = null!;
}
