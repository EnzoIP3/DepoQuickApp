namespace HomeConnect.WebApi.Controllers.Homes.Models;

public sealed record CreateRoomRequest
{
    public string? Name { get; set; }
}
