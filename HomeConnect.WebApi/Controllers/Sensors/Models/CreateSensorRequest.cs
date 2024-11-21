using BusinessLogic.BusinessOwners.Models;
using BusinessLogic.Users.Entities;

namespace HomeConnect.WebApi.Controllers.Sensors.Models;

public sealed record CreateSensorRequest
{
    public string? Description { get; set; }
    public string? MainPhoto { get; set; }
    public string? ModelNumber { get; set; }
    public string? Name { get; set; }
    public List<string>? SecondaryPhotos { get; set; }
    public string? Validator { get; set; }

    public CreateDeviceArgs ToArgs(User user)
    {
        return new CreateDeviceArgs
        {
            Owner = user!,
            Description = Description ?? string.Empty,
            MainPhoto = MainPhoto ?? string.Empty,
            ModelNumber = ModelNumber,
            Name = Name ?? string.Empty,
            SecondaryPhotos = SecondaryPhotos,
            Type = "Sensor"
        };
    }
}
