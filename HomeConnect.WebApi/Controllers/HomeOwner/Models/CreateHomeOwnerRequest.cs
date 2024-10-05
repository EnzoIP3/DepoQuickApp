namespace HomeConnect.WebApi.Controllers.HomeOwner.Models;

public struct CreateHomeOwnerRequest
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string ProfilePicture { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
