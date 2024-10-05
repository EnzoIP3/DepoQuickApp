namespace HomeConnect.WebApi.Controllers.Businesses.Models;

public struct CreateBusinessRequest
{
    public string Name { get; set; }
    public string OwnerId { get; set; }
    public string Rut { get; set; }
}
