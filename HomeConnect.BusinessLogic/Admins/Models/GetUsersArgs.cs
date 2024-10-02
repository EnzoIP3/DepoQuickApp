namespace BusinessLogic.Admins.Models;

public struct GetUsersArgs
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Surname { get; init; }
    public string FullName { get; init; }
    public string Role { get; init; }
    public DateOnly CreatedAt { get; init; }
}
