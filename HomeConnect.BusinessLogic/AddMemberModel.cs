namespace BusinessLogic;

public struct AddMemberModel
{
    public string HomeId { get; set; }
    public string HomeOwnerEmail { get; set; }
    public bool CanAddDevices { get; set; }
    public bool CanListDevices { get; set; }
}
