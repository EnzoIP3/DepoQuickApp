namespace HomeConnect.WebApi.Controllers.HomeOwners.Models;

public class ListHomeInfo
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public int? MaxMembers { get; set; }
}
