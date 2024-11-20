namespace HomeConnect.WebApi.Controllers.Homes.Models;

public record AddMemberRequest
{
    public string? Email { get; set; } = null!;
    public List<string> Permissions { get; set; } = [];
}
