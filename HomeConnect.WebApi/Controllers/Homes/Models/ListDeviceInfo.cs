namespace HomeConnect.WebApi.Controllers.Homes.Models;

public record ListDeviceInfo
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string BusinessName { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string ModelNumber { get; set; } = null!;
    public string MainPhoto { get; set; } = null!;
    public List<string> SecondaryPhotos { get; set; } = null!;
}
