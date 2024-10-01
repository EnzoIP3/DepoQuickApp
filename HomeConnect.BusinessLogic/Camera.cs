using BusinessLogic;

public class Camera : Device
{
    private const string CameraType = "Camera";
    public bool MotionDetection { get; set; }
    public bool PersonDetection { get; set; }
    public bool IsExterior { get; set; }
    public bool IsInterior { get; set; }

    public Camera(string name, int modelNumber, string description, string mainPhoto, List<string> secondaryPhotos, Business business,
        bool motionDetection, bool personDetection, bool isExterior, bool isInterior)
        : base(name, modelNumber, description, mainPhoto, secondaryPhotos, CameraType, business)
    {
        MotionDetection = motionDetection;
        PersonDetection = personDetection;
        IsExterior = isExterior;
        IsInterior = isInterior;
        EnsureExteriorOrInterior();
    }

    private void EnsureExteriorOrInterior()
    {
        if (!IsExterior && !IsInterior)
        {
            throw new ArgumentException("Camera must be either exterior or interior");
        }
    }
}
