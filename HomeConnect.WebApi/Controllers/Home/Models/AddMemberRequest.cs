namespace HomeConnect.WebApi.Controllers.Home.Models;

public record AddMemberRequest
{
    public string HomeId { get; set; } = null!;
    public string HomeOwnerId { get; set; } = null!;
    public bool CanAddDevices { get; set; }
    public bool CanListDevices { get; set; }
}
