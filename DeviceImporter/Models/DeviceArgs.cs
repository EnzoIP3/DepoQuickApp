namespace DeviceImporter.Models;

public struct DeviceArgs
{
    public DeviceArgs()
    {
    }

    public string Name { get; set; } = null;
    public string ModelNumber { get; set; } = null;
    public string Description { get; set; } = null;
    public string MainPhoto { get; set; } = null;
    public List<string> SecondaryPhotos { get; set; } = null;
    public string Type { get; set; } = null;
    public bool? MotionDetection { get; set; } = null;
    public bool? PersonDetection { get; set; } = null;
    public bool? IsExterior { get; set; } = false;
    public bool? IsInterior { get; set; } = true;
}
