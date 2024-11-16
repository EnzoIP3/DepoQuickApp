using BusinessLogic.Devices.Entities;

namespace BusinessLogic.HomeOwners.Entities;

public class Room
{
    public Room()
    {
    }

    public Room(string name, Home home)
    {
        Name = name;
        Home = home;
    }

    private string _name = string.Empty;
    private Home _home = null!;
    public Guid Id { get; set; } = Guid.NewGuid();
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
            _home = value ?? throw new ArgumentException("Room must have a home assigned.");
        }
    }

    public List<OwnedDevice> OwnedDevices { get; set; } = [];
}
