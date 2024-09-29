namespace BusinessLogic;

public class Home
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public User Owner { get; set; }

    private string _address = string.Empty;

    public string Address
    {
        get => _address;
        set
        {
            var parts = value.Split(' ');
            if (parts.Length < 2)
            {
                throw new ArgumentException("Address must be road and number");
            }

            if (!parts.Last().All(char.IsDigit))
            {
                throw new ArgumentException("Address must be road and number");
            }

            if (!parts.Any(part => part.All(char.IsLetter)))
            {
                throw new ArgumentException("Address must be road and number");
            }

            _address = value;
        }
    }

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
