namespace BusinessLogic;

public class Notification
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public bool Read { get; set; }
    public string Event { get; set; } = null!;
    public OwnedDevice OwnedDevice { get; set; } = null!;
    public User User { get; set; } = null!;

    public Notification()
    {
    }

    public Notification(Guid id, DateTime date, bool read, string @event, OwnedDevice ownedDevice, User user)
    {
        Id = id;
        Date = date;
        Read = read;
        Event = @event;
        OwnedDevice = ownedDevice;
        User = user;
    }
}
