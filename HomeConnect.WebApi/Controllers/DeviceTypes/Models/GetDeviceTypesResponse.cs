namespace HomeConnect.WebApi.Controllers.DeviceTypes.Models;

public sealed record GetDeviceTypesResponse
{
    public List<string> DeviceTypes { get; set; } = null!;
}
