namespace HomeConnect.WebApi.Controllers.Device.Models;

public record GetDevicesRequest
{
    public int? Page { get; set; }
    public int? PageSize { get; set; }
    public string? DeviceNameFilter { get; set; }
    public int? ModelNumberFilter { get; set; }
    public string? BusinessNameFilter { get; set; }
    public string? DeviceTypeFilter { get; set; }
}
