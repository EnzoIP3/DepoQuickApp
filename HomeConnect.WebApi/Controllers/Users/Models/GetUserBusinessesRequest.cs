namespace HomeConnect.WebApi.Controllers.Users.Models;

public record GetUserBusinessesRequest
{
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
