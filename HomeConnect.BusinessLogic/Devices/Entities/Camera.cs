using BusinessLogic.BusinessOwners.Entities;

namespace BusinessLogic.Devices.Entities;

public class Camera : Device
{
    private const string CameraType = "Camera";

    public Camera()
    {
    }

    public Camera(
        string name,
        string? modelNumber,
        string description,
        string mainPhoto,
        List<string>? secondaryPhotos,
        Business business,
        bool? motionDetection,
        bool? personDetection,
        bool? isExterior,
        bool? isInterior)
        : base(name, modelNumber, description, mainPhoto, secondaryPhotos, CameraType, business)
    {
        ValidateCameraProperties(motionDetection, personDetection, isExterior, isInterior);
        MotionDetection = motionDetection!.Value;
        PersonDetection = personDetection!.Value;
        IsExterior = isExterior!.Value;
        IsInterior = isInterior!.Value;
        EnsureExteriorOrInterior();
    }

    public bool MotionDetection { get; private set; }
    public bool PersonDetection { get; private set; }
    public bool IsExterior { get; private set; }
    public bool IsInterior { get; private set; }

    private void ValidateCameraProperties(
        bool? motionDetection,
        bool? personDetection,
        bool? isExterior,
        bool? isInterior)
    {
        CheckPropertyIsNotNull(motionDetection, "Motion detection must be provided");
        CheckPropertyIsNotNull(personDetection, "Person detection must be provided");
        CheckPropertyIsNotNull(isExterior, "Is exterior must be provided");
        CheckPropertyIsNotNull(isInterior, "Is interior must be provided");
    }

    private void CheckPropertyIsNotNull(bool? property, string errorMessage)
    {
        if (!property.HasValue)
        {
            throw new ArgumentException(errorMessage);
        }
    }

    private void EnsureExteriorOrInterior()
    {
        if (!IsExterior && !IsInterior)
        {
            throw new ArgumentException("Camera must be either exterior or interior.");
        }
    }
}
