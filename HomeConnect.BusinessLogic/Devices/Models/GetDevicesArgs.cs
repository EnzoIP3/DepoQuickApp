namespace BusinessLogic.Devices.Models;

public record GetDevicesArgs
{
    public int? Page { get; set; } = 1;
    public int? PageSize { get; set; } = 20;
    public string? DeviceNameFilter { get; init; }
    public string? ModelNumberFilter { get; init; }
    public string? BusinessNameFilter { get; init; }
    public string? DeviceTypeFilter { get; init; }
    public string? RutFilter { get; init; }
}
