namespace BusinessLogic.Admins.Models;

public record GetBusinessesArgs
{
    public int? CurrentPage { get; set; }
    public int? PageSize { get; set; }
    public string? NameFilter { get; set; }
    public string? FullNameFilter { get; set; }
}
