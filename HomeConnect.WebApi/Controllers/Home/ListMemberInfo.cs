namespace HomeConnect.WebApi.Controllers.Home;

public struct ListMemberInfo
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Photo { get; set; }
    public bool CanAddDevices { get; set; }
    public bool CanListDevices { get; set; }
    public bool ShouldBeNotified { get; set; }
}
