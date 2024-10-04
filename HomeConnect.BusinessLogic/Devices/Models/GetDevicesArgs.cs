namespace BusinessLogic.Devices.Models;

public struct GetDevicesArgs
{
    public string Name { get; init; }
    public int ModelNumber { get; init; }
    public string Description { get; init; }
    public string MainPhoto { get; init; }
    public string Type { get; init; }
    public string BusinessName { get; init; }
    public string OwnerEmail { get; init; }
}
