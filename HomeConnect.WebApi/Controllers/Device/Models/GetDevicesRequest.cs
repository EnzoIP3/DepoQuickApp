namespace HomeConnect.WebApi.Controllers.Device.Models;

public record GetDevicesRequest
{
    public string? Name { get; set; }
    public int? Model { get; set; }
    public string? BusinessName { get; set; }
    public string? Type { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
