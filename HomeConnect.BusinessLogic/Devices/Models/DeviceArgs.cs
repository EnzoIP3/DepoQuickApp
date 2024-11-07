namespace BusinessLogic.Devices.Services;

public struct DeviceArgs
{
    public string Name { get; set; }
    public string ModelNumber { get; set; }
    public string Description { get; set; }
    public string MainPhoto { get; set; }
    public List<string> SecondaryPhotos { get; set; }
    public string Type { get; set; }
    public bool? MotionDetection { get; set; }
    public bool? PersonDetection { get; set; }
    public bool? IsExterior { get; set; }
    public bool? IsInterior { get; set; }
}
