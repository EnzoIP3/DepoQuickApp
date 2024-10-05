namespace HomeConnect.WebApi.Controllers.BusinessOwner.Models;

public struct CreateDeviceRequest
{
    public string BusinessRut { get; set; }
    public string Description { get; set; }
    public string MainPhoto { get; set; }
    public int ModelNumber { get; set; }
    public string Name { get; set; }
    public List<string> SecondaryPhotos { get; set; }
    public string Type { get; set; }
}
