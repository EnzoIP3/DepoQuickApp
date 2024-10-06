using BusinessLogic.Users.Entities;

namespace BusinessLogic.BusinessOwners.Models;

public record CreateCameraArgs
{
    public string Name { get; set; } = null!;
    public User Owner { get; set; } = null!;
    public int? ModelNumber { get; set; }
    public string Description { get; set; } = null!;
    public string MainPhoto { get; set; } = null!;
    public List<string>? SecondaryPhotos { get; set; }
    public bool? MotionDetection { get; set; }
    public bool? PersonDetection { get; set; }
    public bool? Exterior { get; set; }
    public bool? Interior { get; set; }
}
