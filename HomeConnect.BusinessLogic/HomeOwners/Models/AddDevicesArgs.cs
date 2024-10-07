namespace BusinessLogic.HomeOwners.Models;

public record AddDevicesArgs
{
    public string HomeId { get; set; } = null!;
    public List<string> DeviceIds { get; set; } = null!;
}
