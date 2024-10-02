namespace BusinessLogic.HomeOwners.Models;

public struct AddMemberArgs
{
    public string HomeId { get; set; }
    public string HomeOwnerId { get; set; }
    public bool CanAddDevices { get; set; }
    public bool CanListDevices { get; set; }
}
