using BusinessLogic.Admins.Models;

namespace HomeConnect.WebApi.Controllers.Users.Models;

public record GetUsersRequest
{
    public int? Page { get; init; }
    public int? PageSize { get; init; }
    public string? FullName { get; init; }
    public string? Roles { get; init; }

    public GetUsersArgs ToArgs()
    {
        return new GetUsersArgs
        {
            CurrentPage = Page, PageSize = PageSize, FullNameFilter = FullName, RoleFilter = Roles
        };
    }
}
