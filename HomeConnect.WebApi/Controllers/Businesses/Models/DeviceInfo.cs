namespace HomeConnect.WebApi.Controllers.Businesses.Models;

public sealed record DeviceInfo
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string BusinessName { get; set; } = null!;
    public string ModelNumber { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string MainPhoto { get; set; } = null!;
    public List<string> SecondaryPhotos { get; set; } = [];
    public string Type { get; set; } = null!;
}
