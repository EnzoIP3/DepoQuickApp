namespace BusinessLogic;

public class Home
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public User Owner { get; set; }
    public string Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int MaxMembers { get; set; }

    public Home(User owner, string address, double latitude, double longitude, int maxMembers)
    {
        Owner = owner;
        Address = address;
        Latitude = latitude;
        Longitude = longitude;
        MaxMembers = maxMembers;
    }
}
