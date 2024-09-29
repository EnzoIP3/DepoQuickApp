namespace BusinessLogic;

public class Home
{
    private string _address = string.Empty;

    public Home(User owner, string address, double latitude, double longitude, int maxMembers)
    {
        Owner = owner;
        Address = address;
        Latitude = latitude;
        Longitude = longitude;
        MaxMembers = maxMembers;
    }

    public Guid Id { get; set; } = Guid.NewGuid();
    public User Owner { get; set; }

    public List<User> Members { get; set; } = new();

    public string Address
    {
        get => _address;
        set
        {
            EnsureAddressHasAtLeastOneSpace(value);
            EnsureAddressContainsRoadName(value);
            EnsureAddressContainsRoadNumber(value);
            _address = value;
        }
    }

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int MaxMembers { get; set; }

    private static void EnsureAddressHasAtLeastOneSpace(string address)
    {
        var parts = address.Split(' ');
        if (parts.Length < 2)
        {
            throw new ArgumentException("Address must be road and number");
        }
    }

    private static void EnsureAddressContainsRoadNumber(string address)
    {
        var parts = address.Split(' ');
        if (!parts.Last().All(char.IsDigit))
        {
            throw new ArgumentException("Address must be road and number");
        }
    }

    private static void EnsureAddressContainsRoadName(string address)
    {
        var parts = address.Split(' ');
        if (!parts.Any(part => part.All(char.IsLetter)))
        {
            throw new ArgumentException("Address must be road and number");
        }
    }

    public void AddMember(User member)
    {
        Members.Add(member);
    }
}
