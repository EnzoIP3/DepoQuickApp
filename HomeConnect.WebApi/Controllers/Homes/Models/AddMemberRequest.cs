namespace HomeConnect.WebApi.Controllers.Home.Models;

public record AddMemberRequest
{
    public string? MemberId { get; set; } = null!;
    public bool CanAddDevices { get; set; }
    public bool CanListDevices { get; set; }
}
