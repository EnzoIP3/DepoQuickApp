namespace HomeConnect.WebApi.Controllers.Users.Models;

public record GetUsersResponse
{
    public List<ListUserInfo> Users { get; init; } = null!;
    public Pagination Pagination { get; init; } = null!;
}
