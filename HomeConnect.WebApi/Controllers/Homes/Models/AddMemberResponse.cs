namespace HomeConnect.WebApi.Controllers.Homes.Models;

public sealed record AddMemberResponse
{
    public string HomeId { get; set; } = null!;
    public string MemberId { get; set; } = null!;
}
