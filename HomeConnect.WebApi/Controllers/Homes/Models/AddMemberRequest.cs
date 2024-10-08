namespace HomeConnect.WebApi.Controllers.Homes.Models;

public record AddMemberRequest
{
    public string? MemberId { get; set; } = null!;
    public bool CanAddDevices { get; set; }
    public bool CanListDevices { get; set; }
}
