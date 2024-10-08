using BusinessLogic.Users.Entities;

namespace BusinessLogic.HomeOwners.Entities;

public class Home
{
    private readonly double _latitude;
    private readonly double _longitude;
    private string _address = string.Empty;

    private int _maxMembers;

    public Home()
    {
    }

    public Home(User owner, string address, double? latitude, double? longitude, int? maxMembers)
    {
        EnsureLatitudeIsNotNull(latitude);
        EnsureLongitudeIsNotNull(longitude);
        EnsureMaxMembersIsNotNull(maxMembers);
        Owner = owner;
        Address = address;
        Latitude = latitude!.Value;
        Longitude = longitude!.Value;
        MaxMembers = maxMembers!.Value;
    }

    public Guid Id { get; set; } = Guid.NewGuid();
    public User Owner { get; set; } = null!;

    public List<Member> Members { get; set; } = [];

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

    public double Latitude
    {
        get => _latitude;
        init
        {
            if (value < -90 || value > 90)
            {
                throw new ArgumentException("Latitude must be between -90 and 90.");
            }

            _latitude = value;
        }
    }

    public double Longitude
    {
        get => _longitude;
        init
        {
            if (value < -180 || value > 180)
            {
                throw new ArgumentException("Longitude must be between -180 and 180.");
            }

            _longitude = value;
        }
    }

    public int MaxMembers
    {
        get => _maxMembers;
        set
        {
            EnsureMaxMembersIsPositive(value);
            _maxMembers = value;
        }
    }

    private void EnsureLatitudeIsNotNull(double? latitude)
    {
        if (latitude == null)
        {
            throw new ArgumentException("Latitude is required.");
        }
    }

    private void EnsureLongitudeIsNotNull(double? longitude)
    {
        if (longitude == null)
        {
            throw new ArgumentException("Longitude is required.");
        }
    }

    private void EnsureMaxMembersIsNotNull(int? maxMembers)
    {
        if (maxMembers == null)
        {
            throw new ArgumentException("Max members is required.");
        }
    }

    private static void EnsureMaxMembersIsPositive(int value)
    {
        if (value < 1)
        {
            throw new ArgumentException("Max members must be at least 1.");
        }
    }

    private static void EnsureAddressHasAtLeastOneSpace(string address)
    {
        var parts = address.Split(' ');
        if (parts.Length < 2)
        {
            throw new ArgumentException("Address must be road and number.");
        }
    }

    private static void EnsureAddressContainsRoadNumber(string address)
    {
        var parts = address.Split(' ');
        if (!parts.Last().All(char.IsDigit))
        {
            throw new ArgumentException("Address must be road and number.");
        }
    }

    private static void EnsureAddressContainsRoadName(string address)
    {
        var parts = address.Split(' ');
        if (!parts.Any(part => part.All(char.IsLetter)))
        {
            throw new ArgumentException("Address must be road and number.");
        }
    }

    public void AddMember(Member member)
    {
        EnsureMemberIsNotOwner(member);
        EnsureMemberIsNotAlreadyAdded(member);
        EnsureMaxMembersIsNotReached();
        Members.Add(member);
    }

    private void EnsureMaxMembersIsNotReached()
    {
        if (Members.Count >= MaxMembers)
        {
            throw new InvalidOperationException("This home is already full.");
        }
    }

    private void EnsureMemberIsNotAlreadyAdded(Member member)
    {
        if (Members.Any(m => m == member))
        {
            throw new InvalidOperationException("The member is already added to this home.");
        }
    }

    private void EnsureMemberIsNotOwner(Member member)
    {
        if (member.User == Owner)
        {
            throw new ArgumentException("Owner cannot be added as member.");
        }
    }
}
