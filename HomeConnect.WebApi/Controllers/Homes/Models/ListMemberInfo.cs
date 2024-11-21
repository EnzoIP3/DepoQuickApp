namespace HomeConnect.WebApi.Controllers.Homes.Models;

public sealed record ListMemberInfo
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Photo { get; set; } = null!;
    public List<string> Permissions { get; set; } = null!;
}
