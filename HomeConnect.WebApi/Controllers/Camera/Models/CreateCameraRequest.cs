namespace HomeConnect.WebApi.Controllers.Camera.Models;

public record CreateCameraRequest
{
    public string BusinessRut { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string MainPhoto { get; set; } = null!;
    public int ModelNumber { get; set; }
    public string Name { get; set; } = null!;
    public List<string> SecondaryPhotos { get; set; } = null!;
    public bool MotionDetection { get; set; }
    public bool PersonDetection { get; set; }
    public bool IsExterior { get; set; }
    public bool IsInterior { get; set; }
}
