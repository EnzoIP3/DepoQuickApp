namespace BusinessLogic;

public struct CreateHomeModel
{
    public string HomeOwnerEmail { get; set; }
    public string Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int MaxMembers { get; set; }
}
