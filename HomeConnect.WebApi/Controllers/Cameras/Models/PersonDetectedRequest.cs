namespace HomeConnect.WebApi.Controllers.Cameras.Models;

public sealed record PersonDetectedRequest
{
    public string? UserEmail { get; set; } = null!;
}
