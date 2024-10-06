namespace HomeConnect.WebApi.Controllers.User.Models;

public record GetUsersRequest
{
    public int? CurrentPage { get; init; }
    public int? PageSize { get; init; }
    public string? FullNameFilter { get; init; }
    public string? RoleFilter { get; init; }
}
