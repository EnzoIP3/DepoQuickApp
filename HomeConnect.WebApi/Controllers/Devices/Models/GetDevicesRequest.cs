namespace HomeConnect.WebApi.Controllers.Devices.Models;

public record GetDevicesRequest
{
    public string? Name { get; set; }
    public string? Model { get; set; }
    public string? BusinessName { get; set; }
    public string? Type { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
