namespace BusinessLogic.HomeOwners.Models;

public record RoomDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string HomeId { get; set; } = null!;
    public List<string> OwnedDevicesId { get; set; } = [];
}
