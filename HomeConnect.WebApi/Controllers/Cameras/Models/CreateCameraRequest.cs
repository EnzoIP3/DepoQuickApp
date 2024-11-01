namespace HomeConnect.WebApi.Controllers.Cameras.Models;

public record CreateCameraRequest
{
    public string? Description { get; set; } = null!;
    public string? MainPhoto { get; set; } = null!;
    public string? ModelNumber { get; set; }
    public string? Name { get; set; } = null!;
    public List<string>? SecondaryPhotos { get; set; }
    public bool? MotionDetection { get; set; }
    public bool? PersonDetection { get; set; }
    public bool? Exterior { get; set; }
    public bool? Interior { get; set; }
}
