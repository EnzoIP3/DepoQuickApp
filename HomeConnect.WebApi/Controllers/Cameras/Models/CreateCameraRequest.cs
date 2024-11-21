using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.Users.Entities;

namespace HomeConnect.WebApi.Controllers.Cameras.Models;

public sealed record CreateCameraRequest
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
    public string? Validator { get; set; }

    public CreateCameraArgs ToCreateCameraArgs(User? owner)
    {
        return new CreateCameraArgs
        {
            Owner = owner!,
            Name = Name ?? string.Empty,
            Description = Description ?? string.Empty,
            Exterior = Exterior,
            Interior = Interior,
            MainPhoto = MainPhoto ?? string.Empty,
            ModelNumber = ModelNumber,
            MotionDetection = MotionDetection,
            PersonDetection = PersonDetection,
            SecondaryPhotos = SecondaryPhotos
        };
    }
}
