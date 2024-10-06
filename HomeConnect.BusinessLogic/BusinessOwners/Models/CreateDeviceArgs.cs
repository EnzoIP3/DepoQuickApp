namespace BusinessLogic.BusinessOwners.Models;

public record CreateDeviceArgs
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int ModelNumber { get; set; }
    public string MainPhoto { get; set; } = null!;
    public List<string> SecondaryPhotos { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string BusinessRut { get; set; } = null!;
}
