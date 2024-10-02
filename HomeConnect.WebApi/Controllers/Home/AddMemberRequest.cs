namespace HomeConnect.WebApi.Controllers.Home;

public struct AddMemberRequest
{
    public string HomeId { get; set; }
    public string HomeOwnerId { get; set; }
    public bool CanAddDevices { get; set; }
    public bool CanListDevices { get; set; }
}
