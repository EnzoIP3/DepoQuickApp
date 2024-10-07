namespace HomeConnect.WebApi.Controllers.Home.Models;

public record ListDeviceInfo
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string BusinessName { get; set; } = null!;
    public string Type { get; set; } = null!;
    public int ModelNumber { get; set; }
    public string Photo { get; set; } = null!;
    public bool IsConnected { get; set; }
}
