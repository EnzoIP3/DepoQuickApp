namespace HomeConnect.WebApi.Controllers.Businesses.Models;

public struct DeviceInfo
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string BusinessName { get; set; }
    public string ModelNumber { get; set; }
    public string Description { get; set; }
    public string MainPhoto { get; set; }
    public List<string> SecondaryPhotos { get; set; }
    public string Type { get; set; }
}
