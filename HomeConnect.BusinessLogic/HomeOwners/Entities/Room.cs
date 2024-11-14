namespace BusinessLogic.HomeOwners.Entities;

public class Room
{
    private string _name = string.Empty;
    private Home _home = null;
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
            if (value == null)
            {
                throw new ArgumentException("Room must have a home assigned.");
            }

            _home = value;
        }
    }
}
