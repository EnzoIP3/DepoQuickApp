namespace BusinessLogic.HomeOwners.Models;

public struct AddDevicesArgs
{
    public string HomeId { get; set; }
    public IEnumerable<string> DeviceIds { get; set; }
}
