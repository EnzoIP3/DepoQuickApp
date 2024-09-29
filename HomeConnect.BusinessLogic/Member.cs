namespace BusinessLogic;

public class Member
{
    public Guid Id { get; } = Guid.NewGuid();
    public User User { get; init; }
    public List<HomePermission> HomePermissions { get; } = [];

    public Member(User user)
    {
        User = user;
    }
}
