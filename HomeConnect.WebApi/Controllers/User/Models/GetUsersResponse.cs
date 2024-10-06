namespace HomeConnect.WebApi.Controllers.User.Models;

public record GetUsersResponse
{
    public List<ListUserInfo> Users { get; init; } = null!;
    public Pagination Pagination { get; init; } = null!;
}
