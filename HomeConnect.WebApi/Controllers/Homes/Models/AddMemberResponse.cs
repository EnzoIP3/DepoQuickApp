namespace HomeConnect.WebApi.Controllers.Home.Models;

public record AddMemberResponse
{
    public string HomeId { get; set; } = null!;
    public string MemberId { get; set; } = null!;
}
