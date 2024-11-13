namespace BusinessLogic.Devices.Models;

public record OwnedDeviceDto
{
    public string HardwareId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string BusinessName { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string ModelNumber { get; set; } = null!;
    public string Photo { get; set; } = null!;
    public bool? State { get; set; }
    public bool? IsOpen { get; set; }
}
