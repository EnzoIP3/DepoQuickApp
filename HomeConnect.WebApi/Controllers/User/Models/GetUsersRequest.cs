namespace HomeConnect.WebApi.Controllers.User.Models;

public record GetUsersRequest
{
    public int? CurrentPage { get; init; }
    public int? PageSize { get; init; }
    public string? FullName { get; init; }
    public string? Role { get; init; }
}
