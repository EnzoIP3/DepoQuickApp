namespace BusinessLogic;

public record PagedData<T>
{
    public List<T> Data { get; set; } = null!;
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}
