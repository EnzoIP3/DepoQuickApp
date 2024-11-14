namespace HomeConnect.WebApi.Controllers.Homes.Models;

public record AddMemberRequest
{
    public string? Email { get; set; } = null!;
    public bool CanAddDevices { get; set; }
    public bool CanListDevices { get; set; }
}
