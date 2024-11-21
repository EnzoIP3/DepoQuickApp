namespace HomeConnect.WebApi.Controllers.Businesses.Models;

public sealed record UpdateValidatorResponse
{
    public string BusinessRut { get; set; } = null!;
    public string Validator { get; set; } = null!;
}
