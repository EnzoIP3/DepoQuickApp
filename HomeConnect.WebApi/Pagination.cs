namespace HomeConnect.WebApi.Test.Controllers;

public struct Pagination
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}
