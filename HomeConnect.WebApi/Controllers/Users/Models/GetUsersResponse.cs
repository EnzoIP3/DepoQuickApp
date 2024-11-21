using BusinessLogic;
using BusinessLogic.Users.Entities;

namespace HomeConnect.WebApi.Controllers.Users.Models;

public record GetUsersResponse
{
    public List<ListUserInfo> Users { get; init; } = null!;
    public Pagination Pagination { get; init; } = null!;

    public static GetUsersResponse FromUsers(PagedData<User> users)
    {
        return new GetUsersResponse
        {
            Users = users.Data.Select(user => new ListUserInfo
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Surname = user.Surname,
                Roles = user.Roles.Select(r => r.Name).ToList(),
                CreatedAt = user.CreatedAt.ToString()
            }).ToList(),
            Pagination = new Pagination
            {
                Page = users.Page, PageSize = users.PageSize, TotalPages = users.TotalPages
            }
        };
    }
}
