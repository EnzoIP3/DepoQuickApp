namespace BusinessLogic.Admins.Models;

public struct GetUsersArgs
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string FullName { get; set; }
    public string Role { get; set; }
    public DateOnly CreatedAt { get; set; }
}
