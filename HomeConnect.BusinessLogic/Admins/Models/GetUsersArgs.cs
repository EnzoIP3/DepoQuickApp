namespace BusinessLogic.Admins.Models;

public record GetUsersArgs
{
    public int? CurrentPage { get; set; }
    public int? PageSize { get; set; }
    public string? FullNameFilter { get; set; }
    public string? RoleFilter { get; set; }
}
