namespace BusinessLogic.HomeOwners.Models;

public record AddMemberArgs
{
    public string HomeId { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public bool CanAddDevices { get; set; }
    public bool CanListDevices { get; set; }
}
