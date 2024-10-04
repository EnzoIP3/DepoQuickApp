using BusinessLogic.Users.Entities;

namespace BusinessLogic.Tokens.Entities;

public class Token
{
    public static int DurationInHours { get; } = 1;
    public Guid Id { get; init; } = Guid.NewGuid();
    public User User { get; } = null!;
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
