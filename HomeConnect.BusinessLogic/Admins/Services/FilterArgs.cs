namespace BusinessLogic.Admins.Services;

public record FilterArgs
{
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? NameFilter { get; set; } = null;
    public string? FullNameFilter { get; set; } = null;
    public Guid? OwnerIdFilter { get; set; } = null;
}