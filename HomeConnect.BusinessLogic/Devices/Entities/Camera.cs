using BusinessLogic.BusinessOwners.Entities;

namespace BusinessLogic.Devices.Entities;

public class Camera : Device
{
    private const string CameraType = "Camera";
    public bool MotionDetection { get; set; }
    public bool PersonDetection { get; set; }
    public bool IsExterior { get; set; }
    public bool IsInterior { get; set; }

    public Camera()
    {
    }

    private void EnsureMotionDetectionIsNotNull(bool? motionDetection)
    {
        if (motionDetection == null)
        {
            throw new ArgumentException("Motion detection must be provided");
        }
    }

    private void EnsurePersonDetectionIsNotNull(bool? personDetection)
    {
        if (personDetection == null)
        {
            throw new ArgumentException("Person detection must be provided");
        }
    }

    private void EnsureIsExteriorIsNotNull(bool? isExterior)
    {
        if (isExterior == null)
        {
            throw new ArgumentException("Is exterior must be provided");
        }
    }

    private void EnsureIsInteriorIsNotNull(bool? isInterior)
    {
        if (isInterior == null)
        {
            throw new ArgumentException("Is interior must be provided");
        }
    }

    public Camera(string name, int? modelNumber, string description, string mainPhoto, List<string>? secondaryPhotos,
        Business business,
        bool? motionDetection, bool? personDetection, bool? isExterior, bool? isInterior)
        : base(name, modelNumber, description, mainPhoto, secondaryPhotos, CameraType, business)
    {
        EnsurePersonDetectionIsNotNull(personDetection);
        EnsureIsExteriorIsNotNull(isExterior);
        EnsureIsInteriorIsNotNull(isInterior);
        EnsureMotionDetectionIsNotNull(motionDetection);
        MotionDetection = motionDetection!.Value;
        PersonDetection = personDetection!.Value;
        IsExterior = isExterior!.Value;
        IsInterior = isInterior!.Value;
        EnsureExteriorOrInterior();
    }

    private void EnsureExteriorOrInterior()
    {
        if (!IsExterior && !IsInterior)
        {
            throw new ArgumentException("Camera must be either exterior or interior.");
        }
    }
}
