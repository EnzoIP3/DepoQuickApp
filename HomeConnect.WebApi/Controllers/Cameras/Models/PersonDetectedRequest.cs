namespace HomeConnect.WebApi.Controllers.Cameras.Models;

public record PersonDetectedRequest
{
    public string? UserId { get; set; } = null!;
}
