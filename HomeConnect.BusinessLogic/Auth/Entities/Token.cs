using BusinessLogic.Users.Entities;

namespace BusinessLogic.Auth.Entities;

public class Token
{
    public static int DurationInHours { get; } = 1;
    public Guid Id { get; init; } = Guid.NewGuid();
    public User User { get; set; } = null!;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public Token()
    {
    }

    public Token(User user)
    {
        User = user;
    }

    public bool IsExpired()
    {
        return CreatedAt.AddHours(DurationInHours) < DateTime.UtcNow;
    }
}
