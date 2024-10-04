namespace HomeConnect.WebApi.Controllers.BusinessOwner;

public struct CreateDeviceRequest
{
    public BusinessLogic.BusinessOwners.Entities.Business Business { get; set; }
    public string Description { get; set; }
    public string MainPhoto { get; set; }
    public int ModelNumber { get; set; }
    public string Name { get; set; }
    public List<string> SecondaryPhotos { get; set; }
    public string Type { get; set; }
    public Guid Id { get; set; }
}
