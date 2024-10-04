namespace HomeConnect.WebApi.Controllers.Business.Models;

public struct CreateTokenRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}
