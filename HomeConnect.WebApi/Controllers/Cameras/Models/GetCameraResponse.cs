using BusinessLogic.Devices.Entities;

namespace HomeConnect.WebApi.Controllers.Cameras.Models;

public struct GetCameraResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ModelNumber { get; set; }
    public string MainPhoto { get; set; }
    public List<string> SecondaryPhotos { get; set; }
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
            Interior = camera.IsInterior,
        };
    }
}
