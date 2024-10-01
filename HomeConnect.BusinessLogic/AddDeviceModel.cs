namespace BusinessLogic;

public struct AddDeviceModel
{
    public string HomeId { get; set; }
    public IEnumerable<string> DeviceIds { get; set; }
}
