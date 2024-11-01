namespace HomeConnect.WebApi.Controllers.Homes.Models;

public record ListDeviceInfo
{
    public string HardwareId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string BusinessName { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string ModelNumber { get; set; } = null!;
    public string Photo { get; set; } = null!;
}
