namespace BusinessLogic;

public class Notification
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public bool Read { get; set; }
    public string Event { get; set; }
    public OwnedDevice OwnedDevice { get; set; }
    public User User { get; set; }

    public Notification(Guid id, DateTime date, bool read, string @event, OwnedDevice ownedDevice, User user)
    {
        throw new NotImplementedException();
    }
}
