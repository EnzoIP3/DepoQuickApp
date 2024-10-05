namespace HomeConnect.WebApi.Controllers.BusinessOwner.Models;

public struct CreateBusinessRequest
{
    public string Name { get; set; }
    public string OwnerId { get; set; }
    public string Rut { get; set; }
}
