namespace BusinessLogic.BusinessOwners.Models;

public struct CreateDeviceArgs
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int ModelNumber { get; set; }
    public string MainPhoto { get; set; }
    public List<string> SecondaryPhotos { get; set; }
    public string Type { get; set; }
    public string BusinessRut { get; set; }
}
