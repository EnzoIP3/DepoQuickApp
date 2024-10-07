namespace HomeConnect.WebApi.Controllers.Sensor.Models;

public record CreateSensorRequest
{
    public string? Description { get; set; }
    public string? MainPhoto { get; set; }
    public int? ModelNumber { get; set; }
    public string? Name { get; set; }
    public List<string>? SecondaryPhotos { get; set; }
}
