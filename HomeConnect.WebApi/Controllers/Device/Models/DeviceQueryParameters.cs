namespace HomeConnect.WebApi.Controllers.Device.Models;

public class DeviceQueryParameters
{
    public int? Page { get; set; } = 1;
    public int? PageSize { get; set; } = 20;
    public string? DeviceNameFilter { get; set; }
    public int? ModelNumberFilter { get; set; }
    public string? BusinessNameFilter { get; set; }
    public string? DeviceTypeFilter { get; set; }
}
