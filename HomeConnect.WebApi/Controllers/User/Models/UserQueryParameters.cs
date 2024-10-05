namespace HomeConnect.WebApi.Controllers.User.Models;

public class UserQueryParameters
{
    public int? CurrentPage { get; set; }
    public int? PageSize { get; set; }
    public string? FullNameFilter { get; set; }
    public string? RoleFilter { get; set; }
}
