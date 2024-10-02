namespace BusinessLogic.HomeOwners.Models;

public struct CreateHomeArgs
{
    public string HomeOwnerId { get; set; }
    public string Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int MaxMembers { get; set; }
}
