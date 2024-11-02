namespace HomeConnect.WebApi.Controllers.Sensors.Models;

public record CreateSensorRequest
{
    public string? Description { get; set; }
    public string? MainPhoto { get; set; }
    public string? ModelNumber { get; set; }
    public string? Name { get; set; }
    public List<string>? SecondaryPhotos { get; set; }
    public string? Validator { get; set; }
}
