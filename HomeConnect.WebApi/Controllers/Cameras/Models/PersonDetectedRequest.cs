namespace HomeConnect.WebApi.Controllers.Camera.Models;

public record PersonDetectedRequest
{
    public string? UserId { get; set; } = null!;
}
