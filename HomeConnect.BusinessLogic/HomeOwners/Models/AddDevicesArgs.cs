namespace BusinessLogic.HomeOwners.Models;

public record AddDevicesArgs
{
    public string HomeId { get; set; } = null!;
    public IEnumerable<string> DeviceIds { get; set; } = null!;
}
