namespace HomeConnect.WebApi.Controllers.Home.Models;

public record ListMemberInfo
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Photo { get; set; } = null!;
    public bool CanAddDevices { get; set; }
    public bool CanListDevices { get; set; }
    public bool ShouldBeNotified { get; set; }
}
