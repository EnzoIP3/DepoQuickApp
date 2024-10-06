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

    public Camera(string name, int? modelNumber, string description, string mainPhoto, List<string>? secondaryPhotos, Business business,
        bool? motionDetection, bool? personDetection, bool? isExterior, bool? isInterior)
        : base(name, modelNumber, description, mainPhoto, secondaryPhotos, CameraType, business)
    {
        MotionDetection = motionDetection ?? throw new ArgumentException("Motion detection must be provided");
        PersonDetection = personDetection ?? throw new ArgumentException("Person detection must be provided");
        IsExterior = isExterior ?? false;
        IsInterior = isInterior ?? false;
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
