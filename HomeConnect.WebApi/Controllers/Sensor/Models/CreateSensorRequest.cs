namespace HomeConnect.WebApi.Controllers.Sensor.Models;

public record CreateSensorRequest
{
    public string Description { get; set; } = null!;
    public string MainPhoto { get; set; } = null!;
    public int ModelNumber { get; set; }
    public string Name { get; set; } = null!;
    public List<string> SecondaryPhotos { get; set; } = null!;
}
