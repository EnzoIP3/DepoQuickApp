namespace HomeConnect.WebApi.Controllers.Lamps.Models;

public class CreateLampRequest
{
    public string? Description { get; set; }
    public string? MainPhoto { get; set; }
    public string? ModelNumber { get; set; }
    public string? Name { get; set; }
    public List<string>? SecondaryPhotos { get; set; }
}
