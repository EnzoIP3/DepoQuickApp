namespace HomeConnect.WebApi.Controllers.Businesses.Models;

public struct GetBusinessDevicesResponse
{
    public List<DeviceInfo> Devices { get; set; }
    public Pagination Pagination { get; set; }
}
