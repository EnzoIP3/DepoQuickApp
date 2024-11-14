using BusinessLogic.Devices.Entities;

namespace BusinessLogic.HomeOwners.Entities;

public class Room
{
    public Room()
    {
    }

    public Room(Guid id, string name, Home home, ICollection<OwnedDevice>? ownedDevices)
    {
        Id = id;
        Name = name;
        Home = home;
        OwnedDevices = ownedDevices ?? new List<OwnedDevice>();
    }

    private string _name = string.Empty;
    private Home _home = null!;
    public Guid Id { get; set; }
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Room name cannot be null or empty.");
            }

            _name = value;
        }
    }

    public Home Home
    {
        get => _home;
        set
        {
            if (value == null)
            {
                throw new ArgumentException("Room must have a home assigned.");
            }

            _home = value;
        }
    }

    public ICollection<OwnedDevice> OwnedDevices { get; set; } = new List<OwnedDevice>();
}
