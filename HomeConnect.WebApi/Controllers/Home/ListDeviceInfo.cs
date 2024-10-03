namespace HomeConnect.WebApi.Controllers.Home;

public struct ListDeviceInfo
{
    public string Name { get; set; }
    public string Type { get; set; }
    public int ModelNumber { get; set; }
    public string Photo { get; set; }
    public bool IsConnected { get; set; }
}
