using BusinessLogic;
using BusinessLogic.BusinessOwners.Entities;

namespace HomeConnect.WebApi.Controllers.Businesses.Models;

public sealed record GetBusinessesResponse
{
    public List<ListBusinessInfo> Businesses { get; init; } = null!;
    public Pagination Pagination { get; init; } = null!;

    public static GetBusinessesResponse FromBusinesses(PagedData<Business> businesses)
    {
        return new GetBusinessesResponse
        {
            Businesses = businesses.Data.Select(b => new ListBusinessInfo
            {
                Name = b.Name,
                OwnerEmail = b.Owner.Email,
                OwnerName = b.Owner.Name,
                OwnerSurname = b.Owner.Surname,
                Rut = b.Rut,
                Logo = b.Logo
            }).ToList(),
            Pagination = new Pagination
            {
                Page = businesses.Page, PageSize = businesses.PageSize, TotalPages = businesses.TotalPages
            }
        };
    }
}
