namespace BusinessLogic;

public sealed record Pagination
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}
