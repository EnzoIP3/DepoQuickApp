using BusinessLogic.Admins.Models;

namespace HomeConnect.WebApi.Controllers.Businesses.Models;

public record GetBusinessesRequest
{
    public int? CurrentPage { get; init; }
    public int? PageSize { get; init; }
    public string? Name { get; init; }
    public string? OwnerName { get; init; }

    public GetBusinessesArgs ToGetBusinessesArgs()
    {
        return new GetBusinessesArgs
        {
            CurrentPage = CurrentPage, PageSize = PageSize, FullNameFilter = OwnerName, NameFilter = Name
        };
    }
}
