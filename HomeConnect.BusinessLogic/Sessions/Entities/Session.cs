using BusinessLogic.Users.Entities;

namespace BusinessLogic.Sessions.Entities;

public class Session
{
    public Guid Id { get; } = Guid.NewGuid();
    public User User { get; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public Session(User user)
    {
        User = user;
    }

    public bool IsExpired()
    {
        return CreatedAt.AddHours(1) < DateTime.UtcNow;
    }
}
