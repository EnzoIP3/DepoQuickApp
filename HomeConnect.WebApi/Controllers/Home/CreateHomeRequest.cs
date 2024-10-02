namespace HomeConnect.WebApi.Controllers.Home;

public struct CreateHomeRequest
{
    public string Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int MaxMembers { get; set; }
}
