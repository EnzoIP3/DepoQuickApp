namespace BusinessLogic.HomeOwners.Entities;

public class Room
{
    private string _name = string.Empty;
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
}
