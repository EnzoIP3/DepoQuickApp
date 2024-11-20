namespace HomeConnect.WebApi.Controllers.Cameras.Models;

public record PersonDetectedRequest
{
    public string? UserEmail { get; set; } = null!;
}
