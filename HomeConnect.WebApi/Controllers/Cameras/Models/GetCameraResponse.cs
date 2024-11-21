using BusinessLogic.Devices.Entities;

namespace HomeConnect.WebApi.Controllers.Cameras.Models;

public sealed record GetCameraResponse
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ModelNumber { get; set; } = string.Empty;
    public string MainPhoto { get; set; } = string.Empty;
    public List<string> SecondaryPhotos { get; set; } = [];
    public bool MotionDetection { get; set; }
    public bool PersonDetection { get; set; }
    public bool Exterior { get; set; }
    public bool Interior { get; set; }

    public static GetCameraResponse FromCamera(Camera camera)
    {
        return new GetCameraResponse
        {
            Id = camera.Id.ToString(),
            Name = camera.Name,
            Description = camera.Description,
            ModelNumber = camera.ModelNumber,
            MainPhoto = camera.MainPhoto,
            SecondaryPhotos = camera.SecondaryPhotos,
            MotionDetection = camera.MotionDetection,
            PersonDetection = camera.PersonDetection,
            Exterior = camera.IsExterior,
            Interior = camera.IsInterior
        };
    }
}
