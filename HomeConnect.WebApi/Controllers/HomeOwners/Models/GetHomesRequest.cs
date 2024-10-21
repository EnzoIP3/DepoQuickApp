namespace HomeConnect.WebApi.Controllers.HomeOwners.Models;

public class GetHomesRequest
{
    public string? Id { get; set; }
    public string? Address { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public int? MaxMembers { get; set; }
}
